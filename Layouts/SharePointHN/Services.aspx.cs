using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharePointHN.Layouts.SharePointHN
{
   public partial class Services : LayoutsPageBase
{
    // Fields
    public static string connectionString = "Data Source=172.16.33.252;Initial Catalog=logUserActivity;user id=sa;password=P@ssw0rd";

    // Methods
    private static void createTableAndSp(string TableName, string addfieldStr, string fieldsStr, bool isExist)
    {
        string[] strArray3;
        string str6;
        string str14;
        string str = "";
        string str2 = "";
        string str3 = "";
        string str4 = "";
        string[] strArray = addfieldStr.Split(new char[] { ',' });
        string[] strArray2 = fieldsStr.Split(new char[] { ',' });
        foreach (string str5 in strArray)
        {
            if (str5.Length <= 0)
            {
                continue;
            }
            strArray3 = str5.Split(new char[] { ':' });
            str6 = "";
            switch (strArray3[1])
            {
                case "Text":
                    str6 = "nvarchar(255)";
                    break;

                case "Number":
                    str6 = "decimal(18,2)";
                    break;

                case "Note":
                    str6 = "nvarchar(Max)";
                    break;

                case "Date":
                    str6 = "Date";
                    break;

                case "Lookup":
                    str6 = "int";
                    break;

                case "MultiLookup":
                    str6 = "nvarchar(max)";
                    break;

                case "User":
                    str6 = "int";
                    break;

                case "Choice":
                    str6 = "nvarchar(Max)";
                    break;

                case "MultiChoice":
                    str6 = "ntext";
                    break;

                case "RelatedCustomLookupQuery":
                    str6 = "int";
                    break;

                case "MasterDetail":
                    str6 = "int";
                    break;

                case "CustomComputedField":
                    str6 = "nvarchar(255)";
                    break;
            }
            str14 = str;
            str = str14 + strArray3[0] + " " + str6 + " null,";
            str14 = str4;
            str4 = str14 + "@" + strArray3[0] + " " + str6 + ",";
            str2 = str2 + "[" + strArray3[0] + "],";
            str3 = str3 + "@" + strArray3[0] + ",";
        }
        if (str.Length > 1)
        {
            str = str.Substring(0, str.Length - 1);
            str2 = str2.Substring(0, str2.Length - 1);
            str3 = str3.Substring(0, str3.Length - 1);
            str4 = str4.Substring(0, str4.Length - 1);
        }
        string commandText = "";
        string str8 = "";
        if (!isExist)
        {
            commandText = string.Format("create table {0} (id int not null,event nvarchar(20) not null,userId int not null,eventDate datetime not null,{1})", TableName, str);
            str8 = string.Format("CREATE PROCEDURE sp_{0}  @id int ,@event nvarchar(20),@userId int,{1} AS BEGIN \r\n                                        INSERT INTO [dbo].{0} (id,event,userid,eventDate,{2}) VALUES (@id,@event,@userid,GETDATE(),{3}) END", new object[] { TableName, str4, str2, str3 });
        }
        else
        {
            string str9 = "";
            string str10 = "";
            string str11 = "";
            string str12 = "";
            foreach (string str5 in strArray2)
            {
                if (str5.Length <= 0)
                {
                    continue;
                }
                strArray3 = str5.Split(new char[] { ':' });
                str6 = "";
                switch (strArray3[1])
                {
                    case "Text":
                        str6 = "nvarchar(255)";
                        break;

                    case "Number":
                        str6 = "decimal(18,2)";
                        break;

                    case "Note":
                        str6 = "nvarchar(Max)";
                        break;

                    case "Date":
                        str6 = "Date";
                        break;

                    case "Lookup":
                        str6 = "int";
                        break;

                    case "MultiLookup":
                        str6 = "nvarchar(max)";
                        break;

                    case "User":
                        str6 = "int";
                        break;

                    case "Choice":
                        str6 = "nvarchar(Max)";
                        break;

                    case "MultiChoice":
                        str6 = "ntext";
                        break;

                    case "RelatedCustomLookupQuery":
                        str6 = "int";
                        break;

                    case "MasterDetail":
                        str6 = "int";
                        break;

                    case "CustomComputedField":
                        str6 = "nvarchar(255)";
                        break;
                }
                str14 = str9;
                str9 = str14 + strArray3[0] + " " + str6 + " null,";
                str14 = str12;
                str12 = str14 + "@" + strArray3[0] + " " + str6 + ",";
                str10 = str10 + "[" + strArray3[0] + "],";
                str11 = str11 + "@" + strArray3[0] + ",";
            }
            if (str9.Length > 1)
            {
                str9 = str9.Substring(0, str.Length - 1);
                str12 = str12.Substring(0, str12.Length - 1);
                str10 = str10.Substring(0, str10.Length - 1);
                str11 = str11.Substring(0, str11.Length - 1);
                commandText = string.Format("alter table {0}\r\n                                          add {1}\r\n                                        ", TableName, str);
                str8 = string.Format("ALTER PROCEDURE sp_{0} @id int null,@event nvarchar(20),@userId int,{1} AS BEGIN \r\n                                        INSERT INTO [dbo].{0} (id,event,userId,eventDate,{2}) VALUES (@id,@event,@userId,GETDATE(),{3}) END", new object[] { TableName, str12, str10, str11 });
            }
        }
        DataAccessBase base2 = new DataAccessBase(connectionString);
        base2.NonQueryTxt(commandText);
        base2.NonQueryTxt(str8);
    }

    private static void GetPermissionFieldLookup(SPWeb web, string listId, out string permissionField, out string permissionList, out List<int> viewers, out List<int> editors, out int creator)
    {
        int iD = web.CurrentUser.ID;
        int creatId = 0;
        int appr1Id = 0;
        int appr2Id = 0;
        int appr3Id = 0;
        int appr4Id = 0;
        int appr5Id = 0;
        string perField = "";
        string perLookupListName = "";
        string perLookupFieldName = "";
        List<int> viwersIds = new List<int>();
        List<int> editorsIds = new List<int>();
        string perLookupField = "";
        string siteURL = web.Url;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    SPList list = Web.GetList("/Lists/Contracts");
                    SPList list2 = Web.GetList("/Lists/FormPermissions");
                    SPList list3 = Web.Lists[new Guid(listId)];
                    string title = list3.Title;
                    string url = list3.RootFolder.Url;
                    string str3 = url.Substring(url.LastIndexOf("/") + 1);
                    SPQuery query = new SPQuery {
                        Query = string.Format("<Where>\r\n                                                          <Eq>\r\n                                                             <FieldRef Name='ListName' />\r\n                                                             <Value Type='Text'>{0}</Value>\r\n                                                          </Eq>\r\n                                                       </Where>", str3)
                    };
                    SPListItem item = (list2.GetItems(query).Count > 0) ? list2.GetItems(query)[0] : null;
                    if (item != null)
                    {
                        perField = (item["PermissionField"] != null) ? item["PermissionField"].ToString() : "";
                        perLookupListName = (item["PermissionLookupList"] != null) ? item["PermissionLookupList"].ToString() : "";
                        perLookupFieldName = (item["PermissionLookupListField"] != null) ? item["PermissionLookupListField"].ToString() : "";
                        if (perLookupListName != "")
                        {
                            perLookupField = perLookupFieldName;
                        }
                        else
                        {
                            perLookupField = perField;
                        }
                        creatId = (item["Creator"] != null) ? new SPFieldLookupValue(item["Creator"].ToString()).LookupId : 0;
                        appr1Id = (item["Approver1"] != null) ? new SPFieldLookupValue(item["Approver1"].ToString()).LookupId : 0;
                        appr2Id = (item["Approver2"] != null) ? new SPFieldLookupValue(item["Approver2"].ToString()).LookupId : 0;
                        appr3Id = (item["Approver3"] != null) ? new SPFieldLookupValue(item["Approver3"].ToString()).LookupId : 0;
                        appr4Id = (item["Approver4"] != null) ? new SPFieldLookupValue(item["Approver4"].ToString()).LookupId : 0;
                        appr5Id = (item["Approver5"] != null) ? new SPFieldLookupValue(item["Approver5"].ToString()).LookupId : 0;
                        SPFieldLookupValueCollection values = (item["Viewers"] != null) ? new SPFieldLookupValueCollection(item["Viewers"].ToString()) : null;
                        SPFieldLookupValueCollection values2 = (item["Editors"] != null) ? new SPFieldLookupValueCollection(item["Editors"].ToString()) : null;
                        foreach (SPFieldLookupValue value2 in values)
                        {
                            viwersIds.Add(value2.LookupId);
                        }
                        foreach (SPFieldLookupValue value2 in values2)
                        {
                            editorsIds.Add(value2.LookupId);
                        }
                        if (appr1Id != 0)
                        {
                            viwersIds.Add(appr1Id);
                        }
                        if (appr2Id != 0)
                        {
                            viwersIds.Add(appr2Id);
                        }
                        if (appr3Id != 0)
                        {
                            viwersIds.Add(appr3Id);
                        }
                        if (appr4Id != 0)
                        {
                            viwersIds.Add(appr4Id);
                        }
                        if (appr5Id != 0)
                        {
                            viwersIds.Add(appr5Id);
                        }
                    }
                }
            }
        });
        permissionField = perLookupField;
        permissionList = perLookupListName;
        viewers = viwersIds;
        editors = editorsIds;
        creator = creatId;
    }

    private static int GetRelatedUserList(SPWeb web, int userLookupId, bool isList, int contractId, int companyId, int areaId, int currentUserId)
    {
        SPList list = web.GetList("/Lists/Contracts");
        SPList list2 = web.GetList("/Lists/Areas");
        SPList list3 = web.GetList("/Lists/ContractUsers");
        SPListItem item = (contractId > 0) ? list.GetItemById(contractId) : null;
        SPListItem itemById = list3.GetItemById(userLookupId);
        if (userLookupId == 1)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_contractors"].ID;
            }
            return new SPFieldLookupValue(item["ContractorUser"].ToString()).LookupId;
        }
        if (userLookupId == 2)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_engineers"].ID;
            }
            return new SPFieldLookupValue(item["ConsultantUser"].ToString()).LookupId;
        }
        if (userLookupId == 4)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_managers"].ID;
            }
            return new SPFieldLookupValue(item["ManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 5)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_directors"].ID;
            }
            return new SPFieldLookupValue(list2.GetItemById(new SPFieldLookupValue(item["Area"].ToString()).LookupId)["AreaManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 9)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_directors"].ID;
            }
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where>\r\n                                                          <Eq>\r\n                                                                <FieldRef Name='Company' LookupId='TRUE' />\r\n                                                                <Value Type='Lookup'>{0}</Value>\r\n                                                            </Eq>\r\n                                                        </Where>", companyId);
            SPListItem item3 = list2.GetItems(query)[0];
            return new SPFieldLookupValue(item3["AreaManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 12)
        {
            return currentUserId;
        }
        if (userLookupId == 13)
        {
            if (isList)
            {
                return web.Groups[@"jnaser\pmis_experirecorder"].ID;
            }
            return new SPFieldLookupValue(list2.GetItemById(new SPFieldLookupValue(item["Area"].ToString()).LookupId)["ExperienceManager"].ToString()).LookupId;
        }
        if (userLookupId == 0x16)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_directors"].ID;
            }
            return new SPFieldLookupValue(list2.GetItemById(areaId)["CManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 0x17)
        {
            if (isList)
            {
                return web.Groups[@"jnasr\pmis_directors"].ID;
            }
            return new SPFieldLookupValue(list2.GetItemById(areaId)["AreaManagerUser"].ToString()).LookupId;
        }
        return new SPFieldLookupValue(itemById["UserName"].ToString()).LookupId;
    }

    [WebMethod]
    public static string LogUserData(string URL, string OS, string BrowserName, string BrowserVersion, string screenHeight, string screenWidth)
    {
        DataAccessBase base2 = new DataAccessBase(connectionString);
        return base2.ScalarTxt(string.Format("INSERT INTO [dbo].[userActivity]([date],[userId],[pageTitle],[OS],[browserVersion],[browserName],[screenHeight],[screenWidth]) OUTPUT Inserted.ID  VALUES ('{0}',{1},N'{2}','{3}',{4},'{5}',{6},{7})", new object[] { DateTime.Now, SPContext.Current.Web.CurrentUser.ID, URL, OS, BrowserVersion, BrowserName, screenHeight, screenWidth })).ToString();
    }

    private static string MoveItem(SPListItem item, string listName, SPListItem permissionItem)
    {
        SPWeb web = item.Web;
        string url = item.Web.Url;
        SPList parentList = item.ParentList;
        int iD = item.ID;
        int num3 = 0;
        if (permissionItem != null)
        {
            string str4 = (permissionItem["PermissionField"] != null) ? permissionItem["PermissionField"].ToString() : "";
            string strUrl = (permissionItem["PermissionLookupList"] != null) ? permissionItem["PermissionLookupList"].ToString() : "";
            string str6 = (permissionItem["PermissionLookupListField"] != null) ? permissionItem["PermissionLookupListField"].ToString() : "";
            if (strUrl != "")
            {
                SPList list = web.GetList(strUrl);
                int lookupId = new SPFieldLookupValue(item[str6].ToString()).LookupId;
                num3 = new SPFieldLookupValue(list.GetItemById(lookupId)[str4].ToString()).LookupId;
            }
            else
            {
                num3 = new SPFieldLookupValue(item[str4].ToString()).LookupId;
            }
        }
        else if (item.ParentList.Fields.ContainsFieldWithStaticName("Contract"))
        {
            num3 = new SPFieldLookupValue(item["Contract"].ToString()).LookupId;
        }
        web.AllowUnsafeUpdates = true;
        SPFolder folder = web.GetFolder("/Lists/" + listName + "/" + num3.ToString());
        if (!folder.Exists)
        {
            SPListItem item3 = parentList.Items.Add(parentList.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, num3.ToString());
            item3["Title"] = num3;
            item3.Update();
            folder = web.GetFolder(parentList.RootFolder.ServerRelativeUrl + "/" + num3.ToString());
        }
        SPFile file = item.Web.GetFile(item.Url);
        string newUrl = string.Format("{0}/{1}_.000", folder.ServerRelativeUrl, item.ID);
        file.MoveTo(newUrl);
        web.AllowUnsafeUpdates = false;
        return "";
    }

    [WebMethod]
    public static string MoveItemToFolder(string listId)
    {
        SPListItem permissionItem = null;
        SPWeb web = SPContext.Current.Web;
        SPList list = web.Lists[new Guid(listId)];
        string url = list.RootFolder.Url;
        string str5 = url.Substring(url.LastIndexOf("/") + 1);
        SPList list2 = web.GetList("/Lists/FormPermissions");
        SPQuery query = new SPQuery();
        query.Query = string.Format("<Where>\r\n                                                   <Eq>\r\n                                                        <FieldRef Name='ListName' />\r\n                                                        <Value Type='Text'>{0}</Value>\r\n                                                    </Eq>\r\n                                                </Where>", str5);
        SPListItemCollection items = list2.GetItems(query);
        if (items.Count != 0)
        {
            permissionItem = items[0];
        }
        SPQuery query2 = new SPQuery();
        query2.Query = string.Format("<Where>\r\n                            <Neq>\r\n                                <FieldRef Name='FSObjType' />\r\n                                <Value Type='int'>1</Value>\r\n                            </Neq>\r\n                        </Where>", new object[0]);
        query2.ViewAttributes = "Scope='RecursiveAll'";
        SPListItemCollection items2 = list.GetItems(query2);
        foreach (SPListItem item2 in items2)
        {
            MoveItem(item2, str5, permissionItem);
        }
        return "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateLogData("InvoiceCM", "Area:Lookup,CurrentUser:User,StartDate:Date,EndDate:Date,CMNum:Number,InvoiceCM:RelatedCustomLookupQuery,Status:Text,Number:Number,areas:MultiLookup", "", "Data Source=192.168.33.2;Initial Catalog=logUserActivity;user id=sa;password=P@ssw0rd");
    }

    private static string SetListItemPermission(SPListItem Item, int userId, int PermissionID, bool ClearPreviousPermissions)
    {
        string strError = "";
        string siteURL = Item.ParentList.ParentWeb.Url;
        Guid listId = Item.ParentList.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPPrincipal byID;
                    Exception exception;
                    web.AllowUnsafeUpdates = true;
                    SPListItem itemById = web.Lists[listId].GetItemById(Item.ID);
                    if (!itemById.HasUniqueRoleAssignments)
                    {
                        itemById.BreakRoleInheritance(!ClearPreviousPermissions);
                    }
                    try
                    {
                        byID = web.SiteUsers.GetByID(userId);
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        byID = web.SiteGroups.GetByID(userId);
                    }
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(byID);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetById(PermissionID);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                    itemById.RoleAssignments.Remove(byID);
                    itemById.RoleAssignments.Add(roleAssignment);
                    try
                    {
                        itemById.SystemUpdate(false);
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        strError = exception.Message;
                    }
                }
            }
        });
        return strError;
    }

    private static string SetPermissionsFor(SPWeb web, SPListItemCollection col, string permissionField, string permissionList, List<int> viewers, List<int> editors, int creator, bool isFolder)
    {
        string message = "ok";
        try
        {
            foreach (SPListItem item in col)
            {
                int contractId = 0;
                int companyId = 0;
                int areaId = 0;
                if (isFolder)
                {
                    contractId = int.Parse(item["Title"].ToString());
                }
                else if (permissionList != "")
                {
                    SPList list = web.GetList("/" + permissionList);
                    int lookupId = new SPFieldLookupValue(item[permissionField].ToString()).LookupId;
                    contractId = new SPFieldLookupValue(list.GetItemById(lookupId)["Contract"].ToString()).LookupId;
                }
                else if (permissionField != "")
                {
                    contractId = new SPFieldLookupValue(item[permissionField].ToString()).LookupId;
                }
                try
                {
                    companyId = new SPFieldLookupValue(item["Company"].ToString()).LookupId;
                }
                catch
                {
                }
                try
                {
                    areaId = new SPFieldLookupValue(item["Area"].ToString()).LookupId;
                }
                catch
                {
                }
                int num5 = 0;
                if (item.HasUniqueRoleAssignments)
                {
                    web.AllowUnsafeUpdates = true;
                    item.ResetRoleInheritance();
                    web.AllowUnsafeUpdates = false;
                }
                foreach (int num6 in viewers)
                {
                    if (num5 == 0)
                    {
                        SetListItemPermission(item, GetRelatedUserList(web, num6, false, contractId, companyId, areaId, 0), 0x40000002, true);
                    }
                    else
                    {
                        SetListItemPermission(item, GetRelatedUserList(web, num6, false, contractId, companyId, areaId, 0), 0x40000002, false);
                    }
                    num5++;
                }
                foreach (int num6 in editors)
                {
                    if (num5 == 0)
                    {
                        SetListItemPermission(item, GetRelatedUserList(web, num6, false, contractId, companyId, areaId, 0), 0x40000003, true);
                    }
                    else
                    {
                        SetListItemPermission(item, GetRelatedUserList(web, num6, false, contractId, companyId, areaId, 0), 0x40000003, false);
                    }
                    num5++;
                }
                bool clearPreviousPermissions = num5 <= 0;
                int iD = web.Groups["تیم راهبری"].ID;
                int userId = web.Groups["تیم راهبری-ویرایش"].ID;
                SetListItemPermission(item, iD, 0x40000002, clearPreviousPermissions);
                SPList list2 = web.GetList("/Lists/Contracts");
                SPListItem itemById = null;
                SPFieldUserValueCollection values = null;
                if (contractId != 0)
                {
                    itemById = list2.GetItemById(contractId);
                    values = (itemById["Viewers"] != null) ? new SPFieldUserValueCollection(web, itemById["Viewers"].ToString()) : null;
                }
                if (values != null)
                {
                    foreach (SPFieldUserValue value2 in values)
                    {
                        SetListItemPermission(item, value2.LookupId, 0x40000002, false);
                    }
                }
                SetListItemPermission(item, userId, 0x40000003, false);
                if (creator != 0)
                {
                    SetListItemPermission(item, GetRelatedUserList(web, creator, false, contractId, companyId, areaId, 0), 0x4000006b, false);
                }
            }
        }
        catch (Exception exception)
        {
            message = exception.Message;
        }
        return message;
    }

    [WebMethod]
    public static string SetPermissionsListItemsFrom(string type, string listId)
    {
        string str = "ok";
        SPWeb web = SPContext.Current.Web;
        List<int> viewers = new List<int>();
        List<int> editors = new List<int>();
        string permissionField = "";
        string permissionList = "";
        int creator = 0;
        SPList list3 = web.Lists[new Guid(listId)];
        GetPermissionFieldLookup(web, listId, out permissionField, out permissionList, out viewers, out editors, out creator);
        string url = list3.RootFolder.Url;
        string str5 = url.Substring(url.LastIndexOf('/') + 1);
        List<string> list4 = new List<string>();
        SPFieldCollection fields = list3.Fields;
        foreach (SPField field in fields)
        {
            if (field.TypeAsString == "MasterDetail")
            {
                list4.Add(field.GetCustomProperty("ListNameLookup").ToString());
            }
        }
        try
        {
            SPList list = web.GetList("/Lists/" + str5 + "Details");
            if (list != null)
            {
                list4.Add(list.ID.ToString());
            }
        }
        catch (Exception)
        {
        }
        SPQuery query = new SPQuery();
        if (type == "Folder")
        {
            query.Query = string.Format("<Where>\r\n                            <Eq>\r\n                                <FieldRef Name='FSObjType' />\r\n                                <Value Type='int'>1</Value>\r\n                            </Eq>\r\n                        </Where>", new object[0]);
        }
        else
        {
            query.Query = string.Format("<Where>\r\n                            <Neq>\r\n                                <FieldRef Name='FSObjType' />\r\n                                <Value Type='int'>1</Value>\r\n                            </Neq>\r\n                        </Where>", new object[0]);
            query.ViewAttributes = "Scope='RecursiveAll'";
        }
        SPListItemCollection col = list3.GetItems(query);
        str = SetPermissionsFor(web, col, permissionField, permissionList, viewers, editors, creator, type == "Folder");
        if (str == "ok")
        {
            viewers.Add(creator);
            foreach (string str6 in list4)
            {
                SPList list6 = web.Lists[new Guid(str6)];
                SPListItemCollection items2 = null;
                if (type == "Folder")
                {
                    items2 = list6.GetItems(query);
                }
                else
                {
                    items2 = list6.GetItems(query);
                }
                SetPermissionsFor(web, items2, permissionField, permissionList, viewers, editors, 0, type == "Folder");
            }
            SPQuery query2 = new SPQuery();
            query2.Query = string.Format("<Where>\r\n                                            <Or>\r\n                                              <Eq>\r\n                                                 <FieldRef Name='Status' />\r\n                                                 <Value Type='Text'>ثبت موقت</Value>\r\n                                              </Eq>\r\n                                              <Eq>\r\n                                                 <FieldRef Name='Status' />\r\n\r\n                                                 <Value Type='Text'>در انتظار ویرایش</Value>\r\n                                              </Eq>\r\n                                            </Or>\r\n                                           </Where>", new object[0]);
            query2.ViewAttributes = "Scope='RecursiveAll'";
            SPListItemCollection items = list3.GetItems(query2);
            if (items.Count > 0)
            {
                foreach (SPListItem item in items)
                {
                    web.AllowUnsafeUpdates = true;
                    item.ResetRoleInheritance();
                    web.AllowUnsafeUpdates = false;
                    int contractId = 0;
                    if (permissionList != "")
                    {
                        SPList list7 = web.GetList("/" + permissionList);
                        int lookupId = new SPFieldLookupValue(item[permissionField].ToString()).LookupId;
                        contractId = new SPFieldLookupValue(list7.GetItemById(lookupId)["Contract"].ToString()).LookupId;
                    }
                    else if (permissionField != "")
                    {
                        contractId = new SPFieldLookupValue(item[permissionField].ToString()).LookupId;
                    }
                    try
                    {
                        int num4 = new SPFieldLookupValue(item["Company"].ToString()).LookupId;
                    }
                    catch
                    {
                    }
                    try
                    {
                        int num5 = new SPFieldLookupValue(item["Area"].ToString()).LookupId;
                    }
                    catch
                    {
                    }
                    web.AllowUnsafeUpdates = true;
                    SetListItemPermission(item, GetRelatedUserList(web, creator, false, contractId, 0, 0, 0), 0x40000003, false);
                    web.AllowUnsafeUpdates = false;
                }
            }
        }
        return str;
    }

    [WebMethod]
    public static string UpdateLogData(string entityName, string addfields, string deletefields, string connectionString)
    {
        SPWeb web = SPContext.Current.Web;
        SPList list = web.GetList("/Lists/InformationLogs");
        SPQuery query = new SPQuery();
        query.Query = string.Format("<Where> \r\n                                         <Eq>\r\n                                            <FieldRef Name='EntityName' />\r\n                                            <Value Type='Text'>{0}</Value>\r\n                                        </Eq>\r\n                                       </Where>", entityName);
        SPListItemCollection items = list.GetItems(query);
        SPListItem item = (items.Count > 0) ? items[0] : list.AddItem();
        item["EntityName"] = entityName;
        if (item["Fields"] != null)
        {
            item["Fields"] = item["Fields"].ToString() + "," + addfields;
        }
        else
        {
            item["Fields"] = addfields;
        }
        item["ConnectionString"] = connectionString;
        web.AllowUnsafeUpdates = true;
        item.Update();
        web.AllowUnsafeUpdates = false;
        createTableAndSp(entityName, addfields, item["Fields"].ToString(), items.Count > 0);
        return "";
    }

    private static int UpdateTable(string connectionStr, string query)
    {
        int num = 0;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            connection.Open();
            command.CommandText = query;
            num = command.ExecuteNonQuery();
            connection.Close();
        }
        return num;
    }

    [WebMethod]
    public static int UpdateUserData(string id)
    {
        DataAccessBase base2 = new DataAccessBase(connectionString);
        return base2.NonQueryTxt(string.Format("UPDATE [dbo].[userActivity] SET [exitDate] = '{0}' WHERE id={1}", DateTime.Now, id));
    }
}

 


}
