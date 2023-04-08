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

        public UserSetActivityCommand(CurrentUserStore currentUser,
                                      ActivityStore activityStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      INavigationService navigateHomeService, 
                                      INavigationService closeModalNavigationService)
        {
            _currentUser = currentUser;
            _activityStore = activityStore;
            _attendanceRecordStore = attendanceRecordStore;

            _navigateHomeService = navigateHomeService;
            _closeModalNavigation = closeModalNavigationService;
        }

        public UserSetActivityCommand(CurrentUserStore currentUser, 
                                      SelectedDataStore selectedUserStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      INavigationService navigateHomeService, 
                                      INavigationService navigateSpecialActivityService = null)
        {
            _currentUser = currentUser;
            _selectedUserStore = selectedUserStore;
            _attendanceRecordStore = attendanceRecordStore;

            _navigateHomeService = navigateHomeService;
            _navigateSpecialActivityService = navigateSpecialActivityService;
        }

        public UserSetActivityCommand(CurrentUserStore currentUser, 
                                      ActivityStore activityStore,
                                      AttendanceRecordStore attendanceRecordStore,
                                      UserSelectActivitySpecialViewModel userSelectActivitySpecialViewModel, 
                                      INavigationService navigateHomeService, 
                                      INavigationService closeModalNavigation)
        {
            _currentUser = currentUser;
            _activityStore = activityStore;
            _attendanceRecordStore = attendanceRecordStore;

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
                    _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, activity);
                    _navigateHomeService.Navigate();
                }
                
            }
            else if (parameter is string value)
            {
                AttendanceRecord attendanceRecord = _attendanceRecordStore.CurrentAttendanceRecord(_currentUser.User);
                switch (value)
                {
                    case "plan":
                        {
                            Activity afterEnd = _activityStore.GlobalSetting.MainNonWorkActivity;
                            Activity selectedPlan = _userSelectActivitySpecialViewModel.SelectedActivity;
                            DateTime expectedStart;
                            DateTime expectedEnd;
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
                                        expectedEnd = _userSelectActivitySpecialViewModel.EndActivity.AddHours(14);
                                    }
                                    else
                                    {
                                        expectedStart = _userSelectActivitySpecialViewModel.StartActivity.AddHours(14);
                                        expectedEnd = _userSelectActivitySpecialViewModel.EndActivity.AddHours(22);
                                    }
                                }

                            }
                            if (expectedStart < DateTime.Now)
                            {
                                expectedStart = DateTime.Now;
                            }

                            AttendanceRecordDetail attendanceRecordDetail = new AttendanceRecordDetail(expectedStart, expectedEnd, _userSelectActivitySpecialViewModel.Description);

                            _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, selectedPlan, expectedStart, attendanceRecordDetail);
                            _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, afterEnd, expectedEnd);
                            _navigateHomeService.Navigate();
                            _closeModalNavigation.Navigate();
                            break;
                        }

                    case "MoveStart":
                        attendanceRecord.Entry = DateTime.Now;
                        _navigateHomeService.Navigate();
                        _closeModalNavigation.Navigate();
                        break;
                    case "Remove":
                        _attendanceRecordStore.RemoveAttendanceRecord(attendanceRecord);
                        _closeModalNavigation.Navigate();
                        break;
                    case "MoveEnd":
                        AttendanceRecord endOfPlan = _attendanceRecordStore.AttendanceRecords(_currentUser.User).FirstOrDefault(a => a.Entry == attendanceRecord.AttendanceRecordDetail.ExpectedEnd);
                        _attendanceRecordStore.RemoveAttendanceRecord(endOfPlan);
                        attendanceRecord.AttendanceRecordDetail.ExpectedEnd = DateTime.Now;
                        Activity mainActivity = _activityStore.GlobalSetting.MainWorkActivity;
                        _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, mainActivity);
                        _closeModalNavigation.Navigate();
                        break;
                }
            }
        }
    }
}
