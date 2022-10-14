using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AzDO.API.Wrappers.WorkItemTracking.Updates
{
    public sealed class UpdatesCustomWrapper : UpdatesWrapper
    {
        private readonly WorkItemsCustomWrapper workItemsCustomWrapper;

        private const string New = "New";
        private const string Ready = "Ready";
        private const string Development = "Development";
        private const string Done = "Done";

        public UpdatesCustomWrapper()
        {
            workItemsCustomWrapper = new WorkItemsCustomWrapper();
        }

        public DataTable GetCycleTimeFromWorkItems(HashSet<WorkItem> workItems)
        {
            var csvTable = new DataTable();
            int tableRowCount = 1;

            // Column Names
            const string SNo = "S.No.";
            const string Id = "Id";
            const string Type = "Type";
            const string Title = "Title";
            const string State = "State";
            const string AssignedTo = "Assigned To";
            const string QA = "QA";
            const string StoryPoints = "Story Points";
            const string CycleTime = "Cycle Time (In Days)";


            List<string> csvHeaders = new List<string>()
            {
                SNo,
                Id,
                Type,
                Title,
                State,
                AssignedTo,
                QA,
                StoryPoints,
                CycleTime
            };

            foreach (string header in csvHeaders)
            {
                csvTable.Columns.Add(header);
            }

            if (workItems != null && workItems.Count > 0)
            {
                foreach (WorkItem witInfo in workItems)
                {
                    //if (witInfo.Fields[FieldNames.SystemTitle].ToString().Contains("Release Management"))
                    //    continue;

                    string witType = witInfo.Fields[FieldNames.SystemWorkItemType].ToString();

                    DataRow newRow = csvTable.NewRow();
                    try
                    {
                        if (witInfo.Id == 143481)
                            Console.WriteLine();

                        newRow[SNo] = tableRowCount++;
                        newRow[Id] = witInfo.Id;
                        newRow[Type] = witInfo.Fields[FieldNames.SystemWorkItemType];
                        newRow[Title] = witInfo.Fields[FieldNames.SystemTitle];
                        newRow[State] = witInfo.Fields[FieldNames.SystemState];

                        if (witInfo.Fields.ContainsKey(FieldNames.MicrosoftVSTSSchedulingStoryPoints))
                        {
                            newRow[StoryPoints] = Convert.ToInt32(witInfo.Fields[FieldNames.MicrosoftVSTSSchedulingStoryPoints]);
                        }

                        if (witInfo.Fields.ContainsKey(FieldNames.SystemAssignedTo))
                        {
                            IdentityRef identity = (IdentityRef)witInfo.Fields[FieldNames.SystemAssignedTo];
                            newRow[AssignedTo] = identity.DisplayName;
                        }

                        if (witInfo.Fields.ContainsKey("Custom.QA"))
                        {
                            IdentityRef identity = (IdentityRef)witInfo.Fields["Custom.QA"];
                            newRow[QA] = identity.DisplayName;
                        }

                        int days = GetCycleDaysForAWorkItem((int)witInfo.Id, out bool isWorkItemDone);

                        if (isWorkItemDone)
                            newRow[CycleTime] = days + " days";
                        else if (days != -1 && days != -2 && !isWorkItemDone)
                            newRow[CycleTime] = days + " days and counting";
                        else if (days == -1)
                            newRow[CycleTime] = "NA";
                        else if (days == -2)
                            newRow[CycleTime] = "Re-opened";

                        csvTable.Rows.Add(newRow);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        csvTable.Rows.Add(newRow);
                        //throw;
                    }
                }
            }

            return csvTable;
        }

        private int CountWeekDays(DateTime d0, DateTime d1)
        {
            int ndays = 1 + Convert.ToInt32((d1 - d0).TotalDays);
            int nsaturdays = (ndays + Convert.ToInt32(d0.DayOfWeek)) / 7;
            int days = ndays - 2 * nsaturdays
                   - (d0.DayOfWeek == DayOfWeek.Sunday ? 1 : 0)
                   + (d1.DayOfWeek == DayOfWeek.Saturday ? 1 : 0);

            return days;
        }

        private int GetCycleDaysForAWorkItem(int workItemId, out bool isWorkItemDone)
        {
            isWorkItemDone = false;
            var stateDates = new Dictionary<string, DateTime>();

            WorkItem storyInfo = workItemsCustomWrapper.GetWorkItem(workItemId);

            if (storyInfo.Fields != null && storyInfo.Fields.ContainsKey(FieldNames.SystemState))
            {
                if (storyInfo.Fields[FieldNames.SystemState].Equals("Ready"))
                {
                    return -1;
                }
            }

            List<WorkItemUpdate> workItemUpdates = GetUpdates(workItemId);

            foreach (WorkItemUpdate witUpdate in workItemUpdates)
            {
                if (witUpdate.Fields != null && witUpdate.Fields.ContainsKey(FieldNames.SystemBoardColumn) && witUpdate.Fields[FieldNames.SystemBoardColumn] != null)
                {
                    if (witUpdate.Fields[FieldNames.SystemBoardColumn].NewValue != null)
                    {
                        if (witUpdate.Fields[FieldNames.SystemBoardColumn].NewValue.Equals(Development))
                        {
                            var devStartDate = Convert.ToDateTime(witUpdate.Fields[FieldNames.MicrosoftVSTSCommonStateChangeDate].NewValue);
                            if (stateDates.ContainsKey(Development))
                                stateDates[Development] = devStartDate;
                            else
                                stateDates.Add(Development, devStartDate);
                        }
                        if (witUpdate.Fields[FieldNames.SystemBoardColumn].NewValue.Equals(Done))
                        {
                            var doneStartDate = Convert.ToDateTime(witUpdate.Fields[FieldNames.MicrosoftVSTSCommonStateChangeDate].NewValue);
                            if (stateDates.ContainsKey(Done))
                                stateDates[Done] = doneStartDate;
                            else
                                stateDates.Add(Done, doneStartDate);
                        }
                    }
                }
            }

            if (stateDates.ContainsKey(Done) && stateDates.ContainsKey(Development))
            {
                if (stateDates[Development] > stateDates[Done])
                {
                    return -2;
                }
                else
                {
                    isWorkItemDone = true;
                    if (stateDates[Development].Month != stateDates[Done].Month)
                    {
                        var endOfMonth = new DateTime(stateDates[Development].Year, stateDates[Development].Month, DateTime.DaysInMonth(stateDates[Development].Year, stateDates[Development].Month));
                        var firstDayOfMonth = new DateTime(stateDates[Done].Year, stateDates[Done].Month, 1);

                        int days1 = CountWeekDays(stateDates[Development], endOfMonth);
                        int days2 = CountWeekDays(firstDayOfMonth, stateDates[Done]);
                        int final = days1 + days2;
                        return final;

                    }
                    return CountWeekDays(stateDates[Development], stateDates[Done]);
                }
            }
            else if (stateDates.ContainsKey(Development))
            {
                if (stateDates[Development].Month != DateTime.Now.Month)
                {
                    var endOfMonth = new DateTime(stateDates[Development].Year, stateDates[Development].Month, DateTime.DaysInMonth(stateDates[Development].Year, stateDates[Development].Month));
                    var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    int days1 = CountWeekDays(stateDates[Development], endOfMonth);
                    int days2 = CountWeekDays(firstDayOfMonth, DateTime.Now);
                    int final = days1 + days2;
                    return final;
                }

                return CountWeekDays(stateDates[Development], DateTime.Now);
            }

            return -1;
        }
    }
}
