using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Classes
{
    [DataContract]
    public class data_POWaitingForApprove
    {
        [DataMember]
        public string UID;
        [DataMember]
        public string Batch;
        [DataMember]
        public string No;
        [DataMember]
        public string Type;
        [DataMember]
        public string Action_Message;
        [DataMember]
        public bool? Accept_Action_Message;
        [DataMember]
        public string Description;
        [DataMember]
        public string Remark;
        [DataMember]
        public string Location_Code;
        [DataMember]
        public decimal? Original_Quantity;
        [DataMember]
        public decimal? Quantity;
        [DataMember]
        public string Unit_of_Measure_Code;
        [DataMember]
        public decimal? Direct_Unit_Cost;
        [DataMember]
        public DateTime? Due_Date;
        [DataMember]
        public string Vendor_No;
        [DataMember]
        public string Vendor_Item_No;
        [DataMember]
        public string Replenishment_System;
        [DataMember]
        public string Status;
        [DataMember]
        public string Comment;
        [DataMember]
        public string ConvertType;
        [DataMember]
        public bool? Converted;
        [DataMember]
        public string CreatedBy;
        [DataMember]
        public DateTime? CreatedOn;
        [DataMember]
        public string UpdatedBy;
        [DataMember]
        public DateTime? UpdatedOn;
        [DataMember]
        public string ApprovedBy;
        [DataMember]
        public DateTime? ApprovedOn;
        [DataMember]
        public string ChangeLog;

        public data_POWaitingForApprove()
        {
        }

        public data_POWaitingForApprove(DAL.LinqToSql.POWaitingForApprove data)
        {
            this.UID = data.UID.ToString();
            this.Batch = data.Batch;
            this.No = data.No;
            this.Type = data.Type;
            this.Action_Message = data.Action_Message;
            this.Accept_Action_Message = data.Accept_Action_Message;
            this.Description = data.Description;
            this.Remark = data.Remark;
            this.Location_Code = data.Location_Code;
            this.Original_Quantity = data.Original_Quantity;
            this.Quantity = data.Quantity;
            this.Unit_of_Measure_Code = data.Unit_of_Measure_Code;
            this.Direct_Unit_Cost = data.Direct_Unit_Cost;
            this.Due_Date = data.Due_Date;
            this.Vendor_No = data.Vendor_No;
            this.Vendor_Item_No = data.Vendor_Item_No;
            this.Replenishment_System = data.Replenishment_System;
            this.Status = data.Status;
            this.Comment = data.Comment;
            this.ConvertType = data.ConvertType;
            this.Converted = data.Converted;
            this.CreatedBy = data.CreatedBy;
            this.CreatedOn = data.CreatedOn;
            this.UpdatedBy = data.UpdatedBy;
            this.UpdatedOn = data.UpdatedOn;
            this.ApprovedBy = data.ApprovedBy;
            this.ApprovedOn = data.ApprovedOn;
            this.ChangeLog = data.ChangeLog;
        }
    }
}
