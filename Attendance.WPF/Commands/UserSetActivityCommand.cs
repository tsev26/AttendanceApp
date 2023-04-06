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
        private readonly CurrentUser _currentUser;
        private readonly SelectedUserStore _selectedUserStore;
        private readonly ActivityStore _activityStore;
        private readonly UserSelectActivitySpecialViewModel _userSelectActivitySpecialViewModel;

        public UserSetActivityCommand(CurrentUser currentUser, 
                                      ActivityStore activityStore, 
                                      INavigationService navigateHomeService, 
                                      INavigationService closeModalNavigationService)
        {
            _currentUser = currentUser;
            _activityStore = activityStore;
            _navigateHomeService = navigateHomeService;
            _closeModalNavigation = closeModalNavigationService;
        }

        public UserSetActivityCommand(CurrentUser currentUser, 
                                      SelectedUserStore selectedUserStore, 
                                      INavigationService navigateHomeService, 
                                      INavigationService navigateSpecialActivityService = null)
        {
            _currentUser = currentUser;
            _selectedUserStore = selectedUserStore;
            _navigateHomeService = navigateHomeService;
            _navigateSpecialActivityService = navigateSpecialActivityService;
        }

        public UserSetActivityCommand(CurrentUser currentUser, 
                                      ActivityStore activityStore, 
                                      UserSelectActivitySpecialViewModel userSelectActivitySpecialViewModel, 
                                      INavigationService navigateHomeService, 
                                      INavigationService closeModalNavigation)
        {
            _currentUser = currentUser;
            _activityStore = activityStore;
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
                    _currentUser.AttendanceRecordStore.AddAttendanceRecord(activity);
                    _navigateHomeService.Navigate();
                }
                
            }
            else if (parameter is string value)
            {
                AttendanceRecord attendanceRecord = _currentUser.AttendanceRecordStore.CurrentAttendanceRecord;
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

                            _currentUser.AttendanceRecordStore.AddAttendanceRecord(selectedPlan, expectedStart, attendanceRecordDetail);
                            _currentUser.AttendanceRecordStore.AddAttendanceRecord(afterEnd, expectedEnd);
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
                        _currentUser.AttendanceRecordStore.RemoveAttendanceRecord(attendanceRecord);
                        _closeModalNavigation.Navigate();
                        break;
                    case "MoveEnd":
                        AttendanceRecord endOfPlan = _currentUser.AttendanceRecordStore.AttendanceRecords.FirstOrDefault(a => a.Entry == attendanceRecord.AttendanceRecordDetail.ExpectedEnd);
                        _currentUser.AttendanceRecordStore.RemoveAttendanceRecord(endOfPlan);
                        attendanceRecord.AttendanceRecordDetail.ExpectedEnd = DateTime.Now;
                        Activity mainActivity = _activityStore.GlobalSetting.MainWorkActivity;
                        _currentUser.AttendanceRecordStore.AddAttendanceRecord(mainActivity);
                        _closeModalNavigation.Navigate();
                        break;
                }
            }
        }
    }
}
