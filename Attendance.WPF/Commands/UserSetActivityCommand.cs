using Attendance.Domain.Models;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class UserSetActivityCommand : AsyncCommandBase
    {
        private readonly INavigationService _navigateHomeService;
        private readonly INavigationService _navigateSpecialActivityService;
        private readonly INavigationService _closeModalNavigation;
        private readonly CurrentUserStore _currentUser;
        private readonly SelectedDataStore _selectedUserStore;
        private readonly ActivityStore _activityStore;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly UserSelectActivitySpecialViewModel _userSelectActivitySpecialViewModel;
        private readonly UserPlanViewModel _userPlanViewModel;
        private readonly MessageStore _messageStore;

        public UserSetActivityCommand(CurrentUserStore currentUser,
                                      ActivityStore activityStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      MessageStore messageStore,
                                      INavigationService navigateHomeService, 
                                      INavigationService closeModalNavigationService)
        {
            _currentUser = currentUser;
            _activityStore = activityStore;
            _attendanceRecordStore = attendanceRecordStore;
            _messageStore = messageStore;

            _navigateHomeService = navigateHomeService;
            _closeModalNavigation = closeModalNavigationService;
        }

        public UserSetActivityCommand(CurrentUserStore currentUser, 
                                      SelectedDataStore selectedUserStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      MessageStore messageStore,
                                      INavigationService navigateHomeService)
        {
            _currentUser = currentUser;
            _selectedUserStore = selectedUserStore;
            _attendanceRecordStore = attendanceRecordStore;
            _messageStore = messageStore;

            _navigateHomeService = navigateHomeService;
        }

        public UserSetActivityCommand(CurrentUserStore currentUserStore,
                                      SelectedDataStore selectedUserStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      UserPlanViewModel userPlanViewModel,
                                      MessageStore messageStore,
                                      INavigationService navigateToHome, 
                                      INavigationService navigateSpecialActivity
                                      )
        {
            _currentUser = currentUserStore;
            _selectedUserStore = selectedUserStore;
            _attendanceRecordStore = attendanceRecordStore;
            _userPlanViewModel = userPlanViewModel;
            _messageStore = messageStore;

            _navigateHomeService = navigateToHome;
            _navigateSpecialActivityService = navigateSpecialActivity;
        }

        public UserSetActivityCommand(CurrentUserStore currentUser, 
                                      ActivityStore activityStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      UserSelectActivitySpecialViewModel userSelectActivitySpecialViewModel,
                                      MessageStore messageStore,
                                      INavigationService navigateHomeService, 
                                      INavigationService closeModalNavigation)
        {
            _currentUser = currentUser;
            _activityStore = activityStore;
            _attendanceRecordStore = attendanceRecordStore;
            _messageStore = messageStore;

            _navigateHomeService = navigateHomeService;
            _closeModalNavigation = closeModalNavigation;
            _userSelectActivitySpecialViewModel = userSelectActivitySpecialViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is Activity activity)
            {
                if (activity.Property.IsPlan)
                {
                    _selectedUserStore.SelectedActivity = activity;
                    _navigateSpecialActivityService.Navigate();
                }
                else
                {
                    await _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, activity);
                    _navigateHomeService.Navigate(_currentUser.User + " zapsal " + activity);
                }
                
            }
            else if (parameter is string value)
            {
                AttendanceRecord attendanceRecord;
                if (_userPlanViewModel != null)
                {
                    attendanceRecord = _userPlanViewModel.SelectedFuturePlan;
                }
                else
                {
                    attendanceRecord = _attendanceRecordStore.CurrentAttendanceRecord;
                }
                 
                switch (value)
                {
                    case "plan":
                        {
                            Activity afterEnd = _activityStore.GlobalSetting.MainNonWorkActivity;
                            Activity selectedPlan = _userSelectActivitySpecialViewModel.SelectedActivity;
                            DateTime expectedStart;
                            DateTime expectedEnd;
                            DateTime now = DateTime.Now;
                            bool isHalfDay = false;
                            if (selectedPlan.Property.HasTime)
                            {
                                expectedStart = _userSelectActivitySpecialViewModel.StartActivity.AddHours(_userSelectActivitySpecialViewModel.StartHour).AddMinutes(_userSelectActivitySpecialViewModel.StartMinute);
                                expectedEnd = _userSelectActivitySpecialViewModel.EndActivity.AddHours(_userSelectActivitySpecialViewModel.EndHour).AddMinutes(_userSelectActivitySpecialViewModel.EndMinute);
                            }
                            else
                            {
                                if (_userSelectActivitySpecialViewModel.IsFullDayPlan)
                                {
                                    expectedStart = _userSelectActivitySpecialViewModel.StartActivity.AddHours(6);
                                    expectedEnd = _userSelectActivitySpecialViewModel.EndActivity.AddHours(22);
                                }
                                else
                                {
                                    if (_userSelectActivitySpecialViewModel.IsHalfDayStart)
                                    {
                                        expectedStart = _userSelectActivitySpecialViewModel.StartActivity.AddHours(6);
                                        expectedEnd = _userSelectActivitySpecialViewModel.StartActivity.AddHours(14);
                                        isHalfDay = true;
                                    }
                                    else
                                    {
                                        expectedStart = _userSelectActivitySpecialViewModel.StartActivity.AddHours(14);
                                        expectedEnd = _userSelectActivitySpecialViewModel.StartActivity.AddHours(23);
                                        isHalfDay = true;
                                    }
                                }

                            }
                            if (expectedStart < now)
                            {
                                expectedStart = now;
                            }

                            AttendanceRecordDetail attendanceRecordDetail = new AttendanceRecordDetail(expectedStart, expectedEnd, _userSelectActivitySpecialViewModel.Description, isHalfDay);

                            await _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, selectedPlan, expectedStart, attendanceRecordDetail);

                            string currentUserName = _currentUser.User.ToString();
                            _closeModalNavigation.Navigate(currentUserName + " vytvořil plán " + selectedPlan);
                            if (expectedStart == now)
                            {
                                _navigateHomeService.Navigate(currentUserName + " vytvořil a zapsal plán " + selectedPlan);
                            }
                            
                            break;
                        }

                    case "MoveStart":
                        User user = _currentUser.User;
                        Activity activityToSet = attendanceRecord.Activity;
                        await _attendanceRecordStore.RemoveAttendanceRecord(attendanceRecord);
                        
                        await _attendanceRecordStore.AddAttendanceRecord(user, activityToSet, DateTime.Now);
                        _closeModalNavigation.Navigate();
                        _navigateHomeService.Navigate("Začátek plánu " + attendanceRecord.Activity + " posunut na teď");
                        break;
                    case "Remove":
                        if (_userPlanViewModel == null)
                        {
                            _closeModalNavigation.Navigate();
                        }
                        _attendanceRecordStore.RemoveAttendanceRecord(attendanceRecord);
                        _messageStore.Message = "Plán " + attendanceRecord.Activity + " odstraněn";

                        break;
                    case "MoveEnd":
                        AttendanceRecord endOfPlan = _attendanceRecordStore.AttendanceRecords.FirstOrDefault(a => a.Entry == attendanceRecord.AttendanceRecordDetail.ExpectedEnd);
                        await _attendanceRecordStore.RemoveAttendanceRecord(endOfPlan);
                        //attendanceRecord.AttendanceRecordDetail.ExpectedEnd = DateTime.Now;
                        Activity mainActivity = _activityStore.GlobalSetting.MainWorkActivity;
                        await _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, mainActivity);
                        _closeModalNavigation.Navigate("Plán " + attendanceRecord.Activity + " předčasně ukončen");
                        break;
                }
            }
        }
    }
}
