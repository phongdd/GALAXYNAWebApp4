using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Classes;
using DAL.LinqToSql;

namespace BLL.Classes
{
    public class BLL_POWaitingForApprove
    {

        POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();
        public IList<data_POWaitingForApprove> GetAll(string batch, DateTime fromdate, DateTime todate, string username)
        {
            var list_items = from p in db.POWaitingForApproves
                             where p.Batch == batch && p.CreatedOn >= fromdate && p.CreatedOn <= todate
                             orderby p.Due_Date descending
                             select new data_POWaitingForApprove(p);
            return list_items.ToList();
        }

        public string UpdateToApprove(string uid, string username, string newStatus)
        {
            //Checking permission before update status


            //Open => Approved By CM => Approved By F&B (Order(Converted) or Reject or Classify) => To be Deleted
            POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();
            try
            {
                string lichsu = "";

                POWaitingForApprove r = db.POWaitingForApproves.SingleOrDefault(u => u.UID.ToString() == uid);
                if (r == null)
                    return "Object cannot be found";
                lichsu = r.ChangeLog;
                string status_before_update = r.Status;
                if (r.Converted == false)
                {
                    if (r.Status == "Open" && newStatus == "Approved By CM")
                    {
                        lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Status: {2}->{3}.\n", DateTime.Now, username, r.Status, newStatus) + lichsu;
                        r.Status = newStatus;
                        r.ChangeLog = lichsu;
                        db.SubmitChanges();
                        return string.Format("{0}: Change status {1} -> {2}", r.No, status_before_update, newStatus);
                    }
                    else if (r.Status == "Approved By CM" && newStatus == "Approved By F&B")
                    {
                        lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Status: {2}->{3}.\n", DateTime.Now, username, r.Status, newStatus) + lichsu;
                        r.Status = newStatus;
                        r.ChangeLog = lichsu;
                        db.SubmitChanges();
                        return string.Format("{0}: Change status {1} -> {2}", r.No, status_before_update, newStatus);
                    }
                    else { return "You cannot do this action"; }
                }
                else
                {
                    return "Converted equal true, you cannot update!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Reopen(string uid, string username)
        {
            //Open => Approved By CM => Approved By F&B (Order(Converted) or Reject or Classify) => To be Deleted
            POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();
            try
            {
                string lichsu = "";

                POWaitingForApprove r = db.POWaitingForApproves.SingleOrDefault(u => u.UID.ToString() == uid);
                if (r == null)
                    return "Object cannot be found";
                lichsu = r.ChangeLog;
                string status_before_update = r.Status;
                if (r.Converted == false && r.Status != "To be Deleted") // 
                {
                    if (r.Status == "Approved By CM")
                    {
                        lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Status: {2}->{3}.\n", DateTime.Now, username, r.Status, "Open") + lichsu;
                        r.Status = "Open";
                        r.ChangeLog = lichsu;
                        db.SubmitChanges();
                        return string.Format("{0}: Change status {1} -> {2}", r.No, status_before_update, "Open");
                    }
                    else if (r.Status == "Approved By F&B")
                    {
                        lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Status: {2}->{3}.\n", DateTime.Now, username, r.Status, "Approved By CM") + lichsu;
                        r.Status = "Approved By CM";
                        r.ChangeLog = lichsu;
                        db.SubmitChanges();
                        return string.Format("{0}: Change status {1} -> {2}", r.No, status_before_update, "Open");
                    }
                    else { return "You cannot do this action"; }
                }
                else
                {
                    return "Converted equal true or Status equal [To be Deleted], you cannot update!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateConverted(string uid, string convertType, string username)
        {
            POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();
            try
            {
                string lichsu = "";
                POWaitingForApprove r = db.POWaitingForApproves.SingleOrDefault(u => u.UID.ToString() == uid);
                if (r == null)
                    return "Object cannot be found";
                lichsu = r.ChangeLog;

                if (r.Converted == false)
                {
                    if (r.Status == "Approved By F&B")
                    {
                        lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Converted: {2}->{3}.\n", DateTime.Now, username, r.Converted, "True") + lichsu;
                        r.Converted = true;
                        r.ConvertType = convertType;
                        r.Status = "Completed";
                        r.ChangeLog = lichsu;
                        db.SubmitChanges();
                        return string.Format("{0}: Converted to {1}", r.No, convertType);
                    }
                    else { return "You cannot do this action"; }
                }
                else
                {
                    return "Converted equal true, you cannot update!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string InsertComment(string uid, string commentText, string username)
        {
            POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();

            POWaitingForApprove r = db.POWaitingForApproves.SingleOrDefault(p => p.UID.ToString().Equals(uid));
            if (r == null)
                return "Object cannot be found";
            string lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} add comment: {2}.\n", DateTime.Now, username, commentText) + r.Comment;
            r.Comment = lichsu;
            db.SubmitChanges();
            return "";
        }

        public bool CheckPermission(string company, string username, string page, string action)
        {
            PermissionsDataContext db = new PermissionsDataContext();
            var r = (from p in db.Permissions
                    where p.Company == company && p.Username == username && p.Action == action && p.Page == page
                    select p).FirstOrDefault();
            if(r!=null)
                return (bool)r.Allow;
            return false;
        }

        public string TobeDeleted(string uid, string username, string newStatus)
        {
            POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();
            try
            {
                POWaitingForApprove r = db.POWaitingForApproves.SingleOrDefault(u => u.UID.ToString() == uid);
                if (r == null)
                    return "Object cannot be found";

                string lichsu = r.ChangeLog;
                if (r.Converted == false)
                {
                    if (r.Status == "Open")
                    {
                        lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Status: {2}->{3}.\n", DateTime.Now, username, r.Status, newStatus) + lichsu;
                        r.Status = newStatus;
                        r.ChangeLog = lichsu;
                        db.SubmitChanges();
                        return "";
                    }
                    else { return "You cannot do this action"; }
                }
                else
                {
                    return "Converted equal true, you cannot delete!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public data_POWaitingForApprove GetRecIdFromKey(string uid) {
            var record = (from p in db.POWaitingForApproves
                          where p.UID.ToString() == uid select new data_POWaitingForApprove (p)).FirstOrDefault();
                             
            return record;
        }

        public string UpdateData(string uid, data_POWaitingForApprove data, string username)
        {
            POWaitingForApproveDataContext db = new POWaitingForApproveDataContext();
            bool isAddNew = uid == "";

            string lichsu = "";

            POWaitingForApprove dt;

            if (isAddNew && true)
            {
                dt = new POWaitingForApprove();
                dt.UID = Guid.NewGuid();
                dt.CreatedBy = username;
                dt.CreatedOn = DateTime.Now;
                lichsu = string.Format("{0:dd/MM/yyyy hh:mm:ss}: Create by {1}.\n", DateTime.Now, username);
                db.POWaitingForApproves.InsertOnSubmit(dt);
            }
            else {
                dt = db.POWaitingForApproves.SingleOrDefault(p => p.UID.ToString().Equals(uid));
                if (dt == null)
                    return "Object cannot be found";
                else
                    lichsu = dt.ChangeLog;
            }
            if (dt.Batch != data.Batch)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Batch: {2}->{3}.\n",
                    DateTime.Now, username, dt.Batch, data.Batch) + lichsu;
                dt.Batch = data.Batch;
            }
            if (dt.No != data.No)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change No: {2}->{3}.\n",
                    DateTime.Now, username, dt.No, data.No) + lichsu;
                dt.No = data.No;
            }
            if (dt.Type != data.Type)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Type: {2}->{3}.\n",
                    DateTime.Now, username, dt.Type, data.Type) + lichsu;
                dt.Type = data.Type;
            }
            if (dt.Action_Message != data.Action_Message)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Action_Message: {2}->{3}.\n",
                    DateTime.Now, username, dt.Action_Message, data.Action_Message) + lichsu;
                dt.Action_Message = data.Action_Message;
            }
            if (dt.Accept_Action_Message != data.Accept_Action_Message)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Accept_Action_Message: {2}->{3}.\n",
                    DateTime.Now, username, dt.Accept_Action_Message, data.Accept_Action_Message) + lichsu;
                dt.Accept_Action_Message = data.Accept_Action_Message;
            }
            if (dt.Description != data.Description)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Description: {2}->{3}.\n",
                    DateTime.Now, username, dt.Description, data.Description) + lichsu;
                dt.Description = data.Description;
            }
            if (dt.Remark != data.Remark)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Remark: {2}->{3}.\n",
                    DateTime.Now, username, dt.Remark, data.Remark) + lichsu;
                dt.Remark = data.Remark;
            }
            if (dt.Location_Code != data.Location_Code)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Location_Code: {2}->{3}.\n",
                    DateTime.Now, username, dt.Location_Code, data.Location_Code) + lichsu;
                dt.Location_Code = data.Location_Code;
            }

            if (dt.Original_Quantity != data.Original_Quantity)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Original_Quantity: {2}->{3}.\n",
                    DateTime.Now, username, dt.Original_Quantity, data.Original_Quantity) + lichsu;
                dt.Original_Quantity = data.Original_Quantity;
            }
            if (dt.Quantity != data.Quantity)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Quantity: {2}->{3}.\n",
                    DateTime.Now, username, dt.Quantity, data.Quantity) + lichsu;
                dt.Quantity = data.Quantity;
            }
            if (dt.Unit_of_Measure_Code != data.Unit_of_Measure_Code)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Unit_of_Measure_Code: {2}->{3}.\n",
                    DateTime.Now, username, dt.Unit_of_Measure_Code, data.Unit_of_Measure_Code) + lichsu;
                dt.Unit_of_Measure_Code = data.Unit_of_Measure_Code;
            }
            if (dt.Direct_Unit_Cost != data.Direct_Unit_Cost)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Direct_Unit_Cost: {2}->{3}.\n",
                    DateTime.Now, username, dt.Direct_Unit_Cost, data.Direct_Unit_Cost) + lichsu;
                dt.Direct_Unit_Cost = data.Direct_Unit_Cost;
            }
            if (dt.Due_Date != data.Due_Date)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Due_Date: {2}->{3}.\n",
                    DateTime.Now, username, dt.Due_Date, data.Due_Date) + lichsu;
                dt.Due_Date = data.Due_Date;
            }
            if (dt.Vendor_No != data.Vendor_No)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Vendor_No: {2}->{3}.\n",
                    DateTime.Now, username, dt.Vendor_No, data.Vendor_No) + lichsu;
                dt.Vendor_No = data.Vendor_No;
            }
            if (dt.Vendor_Item_No != data.Vendor_Item_No)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Vendor_Item_No: {2}->{3}.\n",
                    DateTime.Now, username, dt.Vendor_Item_No, data.Vendor_Item_No) + lichsu;
                dt.Vendor_Item_No = data.Vendor_Item_No;
            }

            if (dt.Replenishment_System != data.Replenishment_System)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Replenishment_System: {2}->{3}.\n",
                    DateTime.Now, username, dt.Replenishment_System, data.Replenishment_System) + lichsu;
                dt.Replenishment_System = data.Replenishment_System;
            }
            if (dt.Status != data.Status)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Status: {2}->{3}.\n",
                    DateTime.Now, username, dt.Status, data.Status) + lichsu;
                dt.Status = data.Status;
            }
            //Insert/ Update khong insert du lieu comment.
            //if (dt.Comment != data.Comment)
            //{
            //    lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Comment: {2}->{3}.\n",
            //        DateTime.Now, username, dt.Comment, data.Comment) + lichsu;
            //    dt.Comment = data.Comment;
            //}
            if (dt.ConvertType != data.ConvertType)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change ConvertType: {2}->{3}.\n",
                    DateTime.Now, username, dt.ConvertType, data.ConvertType) + lichsu;
                dt.ConvertType = data.ConvertType;
            }
            if (dt.Converted != data.Converted)
            {
                lichsu = String.Format("{0:dd/MM/yyyy hh:mm:ss}: {1} Change Converted: {2}->{3}.\n",
                    DateTime.Now, username, dt.Converted, data.Converted) + lichsu;
                dt.Converted = data.Converted;
            }
            dt.UpdatedBy = username;
            dt.UpdatedOn = DateTime.Now;
            dt.ChangeLog = lichsu;
            db.SubmitChanges();
            return dt.UID.ToString();
        }
    }
}
