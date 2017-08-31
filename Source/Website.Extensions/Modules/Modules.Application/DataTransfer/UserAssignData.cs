using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modules.Application.Database;

namespace Modules.Application.DataTransfer
{
    public class UserAssignData
    {
        public readonly string UserID;
        public readonly string UserName;
        public readonly string PhaseID;
        private readonly List<string> ListPolicy = new List<string>();
        private readonly double Rate;

        public readonly int TotalApplications;
        public int TotalAssigned => ListAssignData.Count;
        public int Total => (int) Math.Round(Rate * (TotalApplications + TotalAssigned));

        private readonly List<string> ListAssignData = new List<string>();
        public string AssignData => string.Join(",", ListAssignData);


        public UserAssignData(DataRow user)
        {
            UserID = user[UserPhaseTable.UserID].ToString();
            UserName = user["UserName"].ToString();
            PhaseID = user[UserPhaseTable.PhaseID].ToString();
            string policyCode = user[UserPhaseTable.PolicyCode].ToString();
            if (string.IsNullOrWhiteSpace(policyCode) == false)
            {
                ListPolicy = policyCode.Split(',').ToList();
            }
            Rate = Math.Round(100 / float.Parse(user[UserPhaseTable.KPI].ToString()), 1);
            TotalApplications = int.Parse(user["TotalApplications"].ToString());
        }


        public void Assign(string applicationID)
        {
            ListAssignData.Add(applicationID);
        }

        public bool Contain(string policyCode)
        {
            return ListPolicy.Count == 0 || ListPolicy.Contains(policyCode);
        }
    }
}