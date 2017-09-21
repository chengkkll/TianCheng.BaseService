using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 对接口进行排序
    /// </summary>
    public class SwaggerOrderBy : IComparer<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(string x, string y)
        {

            return SetIndex(x).CompareTo(SetIndex(y));
        }

        private string SetIndex(string str)
        {
            switch (str)
            {
                case "Auth": return "010" + str;
                case "System": return "011" + str;
                case "DropData": return "012" + str;

                case "Employee": return "020" + str;
                case "Department": return "021" + str;
                case "Function": return "022" + str;
                case "Menu": return "022" + str;
                case "Role": return "023" + str;
                case "Industry": return "029" + str;

                case "Company": return "100" + str;
                case "Linkman": return "101" + str;
                case "Exhibit": return "102" + str;
                case "ExhibitHistory": return "103" + str;

                case "MainExhibition": return "200" + str;
                case "Exhibition": return "201" + str;
                case "ExhibitionPhoto": return "202" + str;
                case "ExhibitionPhotoTags": return "203" + str;
                case "Agency": return "204" + str;

                case "AgainType": return "301" + str;
                case "ExpandingCustomer": return "302" + str;
                case "ContactCompany": return "303" + str;
                case "DepartmentCompany": return "304" + str;
                case "SaleDepartmentSetting": return "305" + str;
                case "SaleEmployeeSetting": return "305" + str;
            }
            return str;
        }

        static Dictionary<string, string> _OrderDict = null;
        static Dictionary<string, string> OrderDict
        {
            get
            {
                if (_OrderDict == null)
                {
                    _OrderDict = new Dictionary<string, string>();
                    _OrderDict.Add("00", "AuthController");
                    _OrderDict.Add("zz", "TianCheng.SystemCommon");
                }
                return _OrderDict;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="des"></param>
        /// <returns></returns>
        static public string Order(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription des)
        {
            if (des.ActionDescriptor != null && !String.IsNullOrWhiteSpace(des.ActionDescriptor.DisplayName))
            {
                string ctrl = des.ActionDescriptor.DisplayName;
                foreach (string key in OrderDict.Keys)
                {
                    if (ctrl.Contains(OrderDict[key]))
                    {
                        return key + ctrl;
                    }
                }
            }
            return des.RelativePath;
        }
    }
}
