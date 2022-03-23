using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities {
    public static class ErrorRegistry {
        ///<summary>Access Denied. You are not an authorized user.</summary>
        public static CustomError E1001 = CustomError.CreateNormalError("1001", @"Access Denied. You are not an authorized user. ({0})");
        ///<summary>System is not available. Please contact system administrator.</summary>
        public static CustomError E2000 = CustomError.CreateNormalError("2000", @"System is not available. Please contact system administrator.");
        ///<summary>Access Denied.</summary>
        public static CustomError E2001 = CustomError.CreateNormalError("2001", @"Access Denied.");
        ///<summary>You are not authorized to perform this operation.</summary>
        public static CustomError E2006 = CustomError.CreateNormalError("2006", @"You are not authorized to perform this operation.");
        ///<summary>You are not authorized to perform this operation in this stage.</summary>
        public static CustomError E2007 = CustomError.CreateNormalError("2007", @"You are not authorized to perform this operation in this stage.");
        ///<summary>You are not authorized to {0} {1} in this stage.</summary>
        public static CustomError E2008 = CustomError.CreateNormalError("2008", @"You are not authorized to {0} {1} in this stage.");
        ///<summary>You are not allowed to perform this operation in this stage because {0}</summary>
        public static CustomError E2009 = CustomError.CreateNormalError("2009", @"You are not allowed to perform this operation in this stage because {0}.");
        ///<summary>There is no such operation for this entity</summary>
        public static CustomError E2010 = CustomError.CreateNormalError("2010", @"There is no such operation for this entity");
        ///<summary>Invalid parameter.  Name: {0}, Value: {1}</summary>
        public static CustomError E2011 = CustomError.CreateNormalError("2011", @"Invalid parameter.  Name: {0}, Value: {1}");
        ///<summary>Invalid parameter.  Name: {0}, Value: {1}</summary>
        public static CustomError X2011 = CustomError.CreateSystemError("2011", @"Invalid parameter.  Name: {0}, Value: {1}");
        ///<summary>Invalid data. Detail: {0}</summary>
        public static CustomError E2012 = CustomError.CreateNormalError("2012", @"Invalid data. Detail: {0}");
        ///<summary>Invalid data. Detail: {0}</summary>
        public static CustomError X2012 = CustomError.CreateSystemError("2012", @"Invalid data. Detail: {0}");
        ///<summary>Invalid attribute.  Name: {0}, Value: {1}</summary>
        public static CustomError E2013 = CustomError.CreateNormalError("2013", @"Invalid attribute.  Name: {0}, Value: {1}");
        ///<summary>Invalid attribute.  Name: {0}, Value: {1}</summary>
        public static CustomError X2013 = CustomError.CreateSystemError("2013", @"Invalid attribute.  Name: {0}, Value: {1}");
        ///<summary>Invalid parameter.  Name: {0}</summary>
        public static CustomError X2014 = CustomError.CreateSystemError("2014", @"Invalid parameter.  Name: {0}");
        ///<summary>Invalid parameter.  {0} is null </summary>
        public static CustomError E2015 = CustomError.CreateNormalError("2015", @"Invalid parameter.  {0} is null");
        ///<summary>Invalid parameter.  {0} is null </summary>
        public static CustomError X2015 = CustomError.CreateSystemError("2015", @"Invalid parameter.  {0} is null");
        ///<summary>Invalid data.  {0} is null or empty</summary>
        public static CustomError X2016 = CustomError.CreateSystemError("2016", @"Invalid data.  {0} is null or empty");
        ///<summary>Converted Data</summary>
        public static CustomError E2017 = CustomError.CreateNormalError("2017", @"Converted Data");

        ///<summary>Instance not found.  Entity: {0}, Key: {1}, Value: {2}</summary>
        public static CustomError X2021 = CustomError.CreateSystemError("2021", @"Instance not found.  Entity: {0}, Key: {1}, Value: {2}");
        ///<summary>More than one instance found.  Entity: {0}, Key: {1}, Value: {2} </summary>
        public static CustomError X2022 = CustomError.CreateSystemError("2022", @"More than one instance found.  Entity: {0}, Key: {1}, Value: {2} ");
        ///<summary>Invalid association.  Source Entity: {0}, Key: {1}, Value: {2}, Target Entity: {3} </summary>
        public static CustomError X2023 = CustomError.CreateSystemError("2023", @"Invalid association.  Source Entity: {0}, Key: {1}, Value: {2}, Target Entity: {3} ");
        ///<summary>Missing association.  Source Entity: {0}, Key: {1}, Value: {2}, Target Entity: {3} </summary>
        public static CustomError X2024 = CustomError.CreateSystemError("2024", @"Missing association.  Source Entity: {0}, Key: {1}, Value: {2}, Target Entity: {3} ");
        ///<summary>Missing parent entity.  Entity: {0}, Key: {1}, Value: {2}, Parent Entity: {3}</summary>
        public static CustomError X2025 = CustomError.CreateSystemError("2025", @"Missing parent entity.  Entity: {0}, Key: {1}, Value: {2}, Parent Entity: {3}");
        ///<summary>Missing one-to-one child entity. Entity: {0}, Key: {1}, Value: {2}, Child Entity: {1}</summary>
        public static CustomError X2026 = CustomError.CreateSystemError("2026", @"Missing one-to-one child entity. Entity: {0}, Key: {1}, Value: {2}, Child Entity: {1}");
        ///<summary>Bulk Operation Incompleted. {0} records processed.</summary>
        public static CustomError E2030 = CustomError.CreateNormalError("2030", @"Bulk Operbation Incompleted. {0} records processed.");
        ///<summary>{0} {1} not found.</summary>
        public static CustomError E2031 = CustomError.CreateNormalError("2031", @"{0} {1} not found.");
        ///<summary>{0} not found.  Key: {1}, Value: {2} </summary>
        public static CustomError E2032 = CustomError.CreateNormalError("2032", @"{0} not found. Key: {1}, Value: {2}");
        ///<summary>{0} {1} already exist.</summary>
        public static CustomError E2033 = CustomError.CreateNormalError("2033", @"{0} {1} already exist.");
        ///<summary>{0} cannot be duplicated. ({1})</summary>
        public static CustomError E2034 = CustomError.CreateNormalError("2034", @"{0} cannot be duplicated. ({1})");
        ///<summary>{0} {1} has already been created by other user.</summary>
        public static CustomError E2035 = CustomError.CreateNormalError("2035", @"{0} {1} has already been created by other user.");
        ///<summary>{0} {1} has already been updated by other user.</summary>
        public static CustomError E2036 = CustomError.CreateNormalError("2036", @"{0} {1} has already been updated by other user.");
        ///<summary>{0} {1} has already been deleted by other user.</summary>
        public static CustomError E2037 = CustomError.CreateNormalError("2037", @"{0} {1} has already been deleted by other user.");
        ///<summary>Instance not found.  Entity: {0}, Key: {1}, Value: {2} and Key: {3}, Value: {4}</summary>
        public static CustomError X2038 = CustomError.CreateSystemError("2038", @"Instance not found.  Entity: {0}, Key: {1}, Value: {2} and Key: {3}, Value: {4}");
        ///<summary>Record has already been updated or deleted by another user.</summary>
        public static CustomError E2039 = CustomError.CreateNormalError("2039", @"Record has already been updated or deleted by another user.");
        ///<summary>You cannot delete yourself.</summary>
        public static CustomError E2040 = CustomError.CreateNormalError("2040", @"You cannot delete yourself.");
        /// <summary>{0} is currently locked by another user, please retry later.</summary>
        public static CustomError E2042 = CustomError.CreateNormalError("2042", @"{0} is currently locked by another user, please retry later.");
        /// <summary>Invalid Status. Entity: {0}, Status: {1}</summary>
        public static CustomError E2043 = CustomError.CreateNormalError("2043", @"Invalid Status. Entity: {0}, Status: {1}");
        ///<summary>Please select at least one {0}.</summary>
        public static CustomError E2044 = CustomError.CreateNormalError("2044", @"Please select at least one {0}.");
        ///<summary>OTP URI is undefined.</summary>
        public static CustomError E2045 = CustomError.CreateNormalError("2045", @"OTP URI is undefined.");
        /// <summary>{0} is in invalid status for {1}. Status: {2}</summary>
        public static CustomError E2046 = CustomError.CreateNormalError("2046", @"{0} is in invalid status for {1}. Status: {2}");

        ///<summary>Delegate: {0} cannot be null.</summary>
        public static CustomError X2046 = CustomError.CreateSystemError("2046", @"Delegate: {0} cannot be null.");
        ///<summary>Entity {0} cannot associate to more than {1} {2}.</summary>
        public static CustomError E2047 = CustomError.CreateNormalError("2047", @"Entity {0} cannot associate to more than {1} {2}.");
        ///<summary>More than {0} records tried. Please refine the search criteria.</summary>
        public static CustomError E2048 = CustomError.CreateNormalError("2048", @"More than {0} records tried. Please refine the search criteria.");
        ///<summary>Cannot perform this operation because {0}.</summary>
        public static CustomError E2049 = CustomError.CreateNormalError("2049", @"Cannot perform this operation because {0}.");
        ///<summary>The {0} does not have child instance of {1}.</summary>
        public static CustomError E2050 = CustomError.CreateNormalError("2050", @"The {0} does not have child instance of {1}.");

        ///<summary>{0} cannot be blank.</summary>
        public static CustomError E2051 = CustomError.CreateNormalError("2051", @"{0} cannot be blank.");
        ///<summary>{0} must be in '{1}' format.</summary>
        public static CustomError E2052 = CustomError.CreateNormalError("2052", @"{0} must be in '{1}' format.");
        ///<summary>{0} must be an integer.</summary>
        public static CustomError E2053 = CustomError.CreateNormalError("2053", @"{0} must be an integer.");
        ///<summary>{0} must be a decimal.</summary>
        public static CustomError E2054 = CustomError.CreateNormalError("2054", @"{0} must be a decimal.");
        ///<summary>{0} must be a decimal.</summary>
        public static CustomError E2086 = CustomError.CreateNormalError("2086", @"{0} must be a datetime.");
        ///<summary>{0} is a decimal with {1} decimal place(s) only.</summary>
        public static CustomError E2055 = CustomError.CreateNormalError("2055", @"{0} is a decimal with {1} decimal place(s) only.");
        ///<summary>{0} must be > {1}.</summary>
        public static CustomError E2056 = CustomError.CreateNormalError("2056", @"{0} must be > {1}.");
        ///<summary>{0} must be >= {1}.</summary>
        public static CustomError E2057 = CustomError.CreateNormalError("2057", @"{0} must be >= {1}.");
        ///<summary>{0} must be &lt; {1}.</summary>
        public static CustomError E2058 = CustomError.CreateNormalError("2058", @"{0} must be < {1}.");
        ///<summary>{0} must be &lt;= {1}.</summary>
        public static CustomError E2059 = CustomError.CreateNormalError("2059", @"{0} must be <= {1}.");
        ///<summary>{0} must be > {1} and &lt; {2}.</summary>
        public static CustomError E2060 = CustomError.CreateNormalError("2060", @"{0} must be > {1} and < {2}.");
        ///<summary>{0} must be >= {1} and &lt; {2}.</summary>
        public static CustomError E2061 = CustomError.CreateNormalError("2061", @"{0} must be >= {1} and < {2}.");
        ///<summary>{0} must be > {1} and &lt;= {2}.</summary>
        public static CustomError E2062 = CustomError.CreateNormalError("2062", @"{0} must be > {1} and <= {2}.");
        ///<summary>{0} must be >= {1} and &lt;= {2}.</summary>
        public static CustomError E2063 = CustomError.CreateNormalError("2063", @"{0} must be >= {1} and <= {2}.");
        ///<summary>Invalid Date. {0} must be in 'dd/MM/yyyy' format.</summary>
        public static CustomError E2064 = CustomError.CreateNormalError("2064", @"Invalid Date. {0} must be in 'dd/MM/yyyy' format.");
        ///<summary>Invalid Date. {0} must be in {1} format.</summary>
        public static CustomError E2064a = CustomError.CreateNormalError("2064a", @"Invalid Date. {0} must be in {1} format.");
        ///<summary>Invalid Time. {0} must be in 'HH:mm' format.</summary>
        public static CustomError E2065 = CustomError.CreateNormalError("2065", @"Invalid Time. {0} must be in 'HH:mm' format.");
        ///<summary>{0} cannot be earlier than 01/01/1900.</summary>
        public static CustomError E2066 = CustomError.CreateNormalError("2066", @"{0} cannot be earlier than 01/01/1900.");
        ///<summary>{0} must be earlier than {1}</summary>
        public static CustomError E2067 = CustomError.CreateNormalError("2067", @"{0} must be earlier than {1}");
        ///<summary>{0} must not be earlier than {1}</summary>
        public static CustomError E2068 = CustomError.CreateNormalError("2068", @"{0} must not be earlier than {1}");
        ///<summary>{0} must be later than {1}</summary>
        public static CustomError E2069 = CustomError.CreateNormalError("2069", @"{0} must be later than {1}.");
        ///<summary>{0} must not be later than {1}</summary>
        public static CustomError E2070 = CustomError.CreateNormalError("2070", @"{0} must not be later than {1}");
        ///<summary>{0} must not be future date</summary>
        public static CustomError E2071 = CustomError.CreateNormalError("2071", @"{0} must not be future date.");
        ///<summary>{0} must not be history date</summary>
        public static CustomError E2072 = CustomError.CreateNormalError("2072", @"{0} must not be history date");
        ///<summary>{0} does not allow Chinese character input.</summary>
        public static CustomError E2073 = CustomError.CreateNormalError("2073", @"{0} does not allow Chinese character input.");
        ///<summary>{0} length must to less than or equal to {1} character(s).</summary>
        public static CustomError E2074 = CustomError.CreateNormalError("2074", @"{0} length must to less than or equal to {1} character(s).");
        ///<summary>Input value is not a valid option for {0}</summary>
        public static CustomError E2075 = CustomError.CreateNormalError("2075", @"Input value is not a valid option for {0}");
        ///<summary>{0} must be blank.</summary>
        public static CustomError E2076 = CustomError.CreateNormalError("2076", @"{0} must be blank.");
        ///<summary>Either one of the following fields must be input: {0}</summary>
        public static CustomError E2077 = CustomError.CreateNormalError("2077", @"Either one of the following fields must be input : {0}");
        ///<summary>Please select {0}.</summary>
        public static CustomError E2078 = CustomError.CreateNormalError("2078", @"Please select {0}.");
        ///<summary>{0} must be unique.</summary>
        public static CustomError E2079 = CustomError.CreateNormalError("2079", @"{0} must be unique.");
        ///<summary>{0} must be unique.</summary>
        public static CustomError X2079 = CustomError.CreateSystemError("2079", @"{0} must be unique.");
        ///<summary>{0} must be equal to {1}.</summary>
        public static CustomError E2080 = CustomError.CreateNormalError("2080", @"{0} must be equal to {1}.");
        ///<summary>{0} must be equal to {1}.</summary>
        public static CustomError X2080 = CustomError.CreateSystemError("2080", @"{0} must be equal to {1}.");
        ///<summary>This record cannot be deleted as it is being referenced. ({0})</summary>
        public static CustomError E2081 = CustomError.CreateNormalError("2081", @"This record cannot be deleted as it is being referenced. ({0})");
        ///<summary>{0} must not be equal to {1}.</summary>
        public static CustomError E2082 = CustomError.CreateNormalError("2082", @"{0} must not be equal to {1}.");
        ///<summary>{0} must not be equal to {1}.</summary>
        public static CustomError X2082 = CustomError.CreateSystemError("2082", @"{0} must not be equal to {1}.");
        ///<summary>{0} must be between {1} and {2}.</summary>
        public static CustomError E2083 = CustomError.CreateNormalError("2083", @"{0} must be between {1} and {2}.");
        ///<summary>{0} must not be between {1} and {2}.</summary>
        public static CustomError E2088 = CustomError.CreateNormalError("2088", @"{0} must not be between {1} and {2}.");
        ///<summary>{0} must be between {1} and {2}.</summary>
        public static CustomError X2083 = CustomError.CreateSystemError("2083", @"{0} must be between {1} and {2}.");
        ///<summary>{0} must not be {1} if {2} is {3}.</summary>
        public static CustomError E2084 = CustomError.CreateNormalError("2084", @"{0} must not be {1} if {2} is {3}.");
        ///<summary>{0} must be {1} if {2} is {3}.</summary>
        public static CustomError E2085 = CustomError.CreateNormalError("2085", @"{0} must be {1} if {2} is {3}.");
        ///<summary>{0} must not contain {1}.</summary>
        public static CustomError E2087 = CustomError.CreateNormalError("2087", @"{0} must not contain {1}.");
        ///<summary>{0} length cannot be more than {1} character(s).</summary>
        public static CustomError E2089 = CustomError.CreateNormalError("E2089", @"{0} length cannot be more than {1} character(s).");
        /// <summary>Internal error. Please contact the system administrator or try again later.</summary>
        public static CustomError X2301 = CustomError.CreateSystemError("2301", @"Internal error. Please contact the system administrator or try again later.");
        /// <summary>Web server is unavailable. Please contact the system administrator or try again later.</summary>
        public static CustomError X2302 = CustomError.CreateSystemError("2302", @"Web server is unavailable. Please contact the system administrator or try again later.");
        /// <summary>Database is unavailable. Please contact the system administrator or try again later.</summary>
        public static CustomError X2303 = CustomError.CreateSystemError("2303", @"Database is unavailable. Please contact the system administrator or try again later.");
        /// <summary>{0} status can not be change</summary>
        public static CustomError E2999 = CustomError.CreateNormalError("2999", @"{0} status can not be change.");
        /// <summary>Constraint conflict.</summary>
        public static CustomError E2305 = CustomError.CreateNormalError("2305", @"Constraint conflict: {0}.");
        /// <summary>.</summary>
        public static CustomError E2099 = CustomError.CreateNormalError("2099", @"{0}.");
        ///<summary>Either one of the following fields must be null or empty:{0}</summary>
        public static CustomError E2090 = CustomError.CreateNormalError("2090", @"Either one of the following fields must be blank : {0}");
        /// <summary>Deadlock conflict.</summary>
        public static CustomError E2306 = CustomError.CreateNormalError("2306", @"Your request to the database record is locked by another user, please retry your operation");

        /// <summary>System Error.</summary>
        public static CustomError X2307 = CustomError.CreateSystemError("2307", @"System error: {0}.");

        /// <summary>Another Application Instance of {0} has already been started.</summary>
        public static CustomError X2310 = CustomError.CreateSystemError("2310", @"Another Application Instance of {0} has already been started.");
        /// <summary>Check update fail. Please restart the application.</summary>
        public static CustomError X2311 = CustomError.CreateSystemError("2311", @"Check update fail. Please restart the application.");

        /// <summary>Timeout expired.</summary>
        public static CustomError E2317 = CustomError.CreateNormalError("2317", @"Timeout expired.");

        ///<summary>Main switch location has conflict in Site Remark</summary>
        public static CustomError E3064 = CustomError.CreateNormalError("3064", @"Main switch location has conflict in Site Remark");
        public static CustomError E5021 = CustomError.CreateNormalError("5021", @"Meter no. is not referenced in the job.");
        public static CustomError E5038 = CustomError.CreateNormalError("5038", @"{0} does not exist");

        /// <summary>
        /// Grouping for ... not supported
        /// </summary>
        public static CustomError E5039 = CustomError.CreateNormalError("5039", @"Grouping for :{0} does not supported.");


        #region For Service Entry Sheet
        ///<summary>{0} must not be negative price.</summary>
        public static CustomError E5003 = CustomError.CreateNormalError("5003", @"{0} must not be negative price.");
        ///<summary>You are exceeding overall limit of the allowed.</summary>
        public static CustomError E5004 = CustomError.CreateNormalError("5004", @"You are exceeding overall limit of the allowed.");
        ///<summary>Same item group must be input same dimension (Line No.: {0}).</summary>
        public static CustomError E5005 = CustomError.CreateNormalError("5005", @"Same item group must be input same dimension (Line No.: {0})");
        ///<summary>Actual amount should not be negative or zero.</summary>
        public static CustomError E5006 = CustomError.CreateNormalError("5006", @"Actual amount should not be negative or zero.");
        ///<summary>Service Entry Sheet is unlock.</summary>
        public static CustomError E5007 = CustomError.CreateNormalError("5007", @"Service Entry Sheet is unlock.");
        ///<summary>The sum of all MO percentage in WO No. {0} and item No. {1} is not equal 100%.</summary>
        public static CustomError E5008 = CustomError.CreateNormalError("5008", @"The sum of all MO percentage in WO No. {0} and item No. {1} is not equal 100%.");

        ///<summary>Can't create service entry sheet because the works order status is not acknowledged or the works order is completed.</summary>
        public static CustomError E5009 = CustomError.CreateNormalError("5009", @"Can't create service entry sheet because the works order status is not in Acknowledged.");
        ///<summary>There are SES exist.</summary>
        public static CustomError E5010 = CustomError.CreateNormalError("5010", @"There are SES exists.");
        ///<summary>Related OA Document not ready for create SES, Please publish OA Document first.</summary>
        public static CustomError E5011 = CustomError.CreateNormalError("5011", @"Related OA Document not ready for create SES, Please publish OA Document first.");
        ///<summary>Price Check Team not found, please confirm the Contract No. is correct.</summary>
        public static CustomError E5012 = CustomError.CreateNormalError("5012", @"Price Check Team not found, please confirm the Contract No. is correct and confirm the Price Check Team is exist.");

        ///<summary>Line No. {0} | {1} must not be blank.</summary>
        public static CustomError E5049 = CustomError.CreateNormalError("5049", @"Line No. {0} | {1} must not be blank.");
        ///<summary>Line No. {0} | The length of {1} must be &lt;= {2}.</summary>
        public static CustomError E5050 = CustomError.CreateNormalError("5050", @"Line No. {0} | The length of {1} must be <= {2}.");
        ///<summary>Line No. {0} | {1} cannot be blank.</summary>
        public static CustomError E5051 = CustomError.CreateNormalError("5051", @"Line No. {0} | {1} cannot be blank.");
        ///<summary>Line No. {0} | {1} not found.</summary>
        public static CustomError E5052 = CustomError.CreateNormalError("5052", @"Line No. {0} | {1} not found.");
        ///<summary>Line No. {0} | Please input {1}.</summary>
        public static CustomError E5053 = CustomError.CreateNormalError("5053", @"Line No. {0} | Please input {1}.");
        ///<summary>Line No. {0} | {1} must be > {1} and &lt;= {2}.</summary>
        public static CustomError E5062 = CustomError.CreateNormalError("5062", @"Line No. {0} | {1} must be > {1} and <= {2}.");
        ///<summary>Line No. {0} | {1} must be >= {2} and &lt;= {3}.</summary>
        public static CustomError E5063 = CustomError.CreateNormalError("5063", @"Line No. {0} | {1} must be >= {2} and <= {3}.");
        ///<summary>Line No. {0} | {1} must be > {1} and &lt; {2}.</summary>
        public static CustomError E5064 = CustomError.CreateNormalError("5064", @"Line No. {0} | {1} must be > {1} and < {2}.");
        ///<summary>Line No. {0} | {1}  should be {2} {3} {4} {5} {6}.</summary>
        public static CustomError E5065 = CustomError.CreateNormalError("5065", @"Line No. {0} | {1}  should be {2} {3} {4} {5} {6}.");
        ///<summary>Line No. {0} | {1} must be > {1} and &lt; {2}.</summary>
        public static CustomError E5066 = CustomError.CreateNormalError("5066", @"{0} must be run first.");
        ///<summary>{0} must not exist.</summary>
        public static CustomError E5067 = CustomError.CreateNormalError("5067", @"{0} must not exist.");
        ///<summary>{0} has already been obsoleted.</summary>
        public static CustomError E5068 = CustomError.CreateNormalError("5068", @"This record has already been obsoleted.");
        ///<summary>{0} must not be null</summary>
        public static CustomError E5069 = CustomError.CreateNormalError("5069", @"{0} must not be null.");
        ///<summary>There are no Service Items exist.</summary>
        public static CustomError E5070 = CustomError.CreateNormalError("5070", @"There are no Service Items exist.");
        ///<summary>The import file total lines is different.</summary>
        public static CustomError E5071 = CustomError.CreateNormalError("5071", @"The import file total lines is different.");
        ///<summary>No service entry sheet existing.</summary>
        public static CustomError E5072 = CustomError.CreateNormalError("5072", @"Service entry sheet does not exist.");
        ///<summary>Please submit all service entry sheet.</summary>
        public static CustomError E5073 = CustomError.CreateNormalError("5073", @"Please submit all service entry sheet.");
        ///<summary>Line {0} | {1} must be > {2}.</summary>
        public static CustomError E5074 = CustomError.CreateNormalError("5074", @"Line No. {0} | {1} must be > {2}.");
        ///<summary>Line {0} | {1} cannot be negative.</summary>
        public static CustomError E5075 = CustomError.CreateNormalError("5075", @"Line No. {0} | {1} cannot be negative.");
        ///<summary>(Line No : {0}) {1} Compare Operator 1 not exist</summary>
        public static CustomError E5076 = CustomError.CreateNormalError("5076", @"(Line No : {0}) {1} Compare Operator 1 not exist.");
        ///<summary>(Line No : {0}) {1} Compare Operator 2 not exist</summary>
        public static CustomError E5077 = CustomError.CreateNormalError("5077", @"(Line No : {0}) {1} Compare Operator 2 not exist.");
        ///<summary>Line No. {0} | {1}  should be {2} {3} and {4} {5}.</summary>
        public static CustomError E5078 = CustomError.CreateNormalError("5078", @"Line No. {0} | {1}  should be {2} {3} and {4} {5}");
        ///<summary>Line No. {0} | {1}  should be {2} {3}.</summary>
        public static CustomError E5079 = CustomError.CreateNormalError("5079", @"Line No. {0} | {1}  should be {2} {3}.");
        ///<summary>Line No. {0} | {1} must be blank.</summary>
        public static CustomError E5081 = CustomError.CreateNormalError("5081", @"Line No. {0} | {1} must be blank.");
        ///<summary>Line No. {0} | {1} must be blank.</summary>
        public static CustomError E5082 = CustomError.CreateNormalError("5082", @"Line No. {0} | {1} is disabled.");
        ///<summary>Record No. {0} | Please fill in all levelling information</summary>
        public static CustomError E5083 = CustomError.CreateNormalError("5083", @"Record No. {0} | Please fill in all levelling information.");
        ///<summary>Record No. {0} | {1} must be > {2}.</summary>
        public static CustomError E5084 = CustomError.CreateNormalError("5084", @"Record No. {0} | {1} must be > {2}.");
        ///<summary>Record No. {0} | Levelling must be L/W/D.</summary>
        public static CustomError E5085 = CustomError.CreateNormalError("5085", @"Record No. {0} | Levelling must be L/W/D.");
        ///<summary>Line No. {0} | {1} should not be zero.</summary>
        public static CustomError E5086 = CustomError.CreateNormalError("5086", @"Line No. {0} | {1} should not be zero.");
        ///<summary>Line No. {0} | {1} should not be zero.</summary>
        public static CustomError E5087 = CustomError.CreateNormalError("5087", @"Line No. {0} | Planned service ({1}) cannot found in contract service item.");
        ///<summary>You are exceeding overall limit (unplanned service) of the allowed. Unplanned overall Limit: {0} , Current Total: {1}.</summary>
        public static CustomError E5088 = CustomError.CreateNormalError("5088", @"You are exceeding overall limit (unplanned service) of the allowed. Unplanned overall Limit: {0} , Current Total: {1}.");
        ///<summary>You are exceeding overall limit (planned service item: {0}) of the allowed. Item Limit: {1} , Current Total: {2}.</summary>
        public static CustomError E5089 = CustomError.CreateNormalError("5089", @"You are exceeding overall limit (planned service item: {0}) of the allowed. Item Limit: {1} , Current Total: {2}.");
        ///<summary>Line No. {0} | {1} must be numeric.</summary>
        public static CustomError E5095 = CustomError.CreateNormalError("5095", @"Line No. {0} | {1} must be numeric.");
        ///<summary>Line No. {0} | {1} must be 2 decimal places only.</summary>
        public static CustomError E5096 = CustomError.CreateNormalError("5096", @"Line No. {0} | {1} must be 2 decimal places only.");

        ///<summary>You are overspent of this Works Order amount limit (Planned amount and {0}% allowed of overspent: {1}, Current Total: {2}.)</summary>
        public static CustomError E5097 = CustomError.CreateNormalError("5097", @"此單已達到超支上限，未能提交。" + Environment.NewLine + "You are exceeded {0}% of planned amount." + Environment.NewLine + Environment.NewLine + "Planned Amount: {1}" + Environment.NewLine + "Overspent Limit: {2}" + Environment.NewLine + "Actual Amount: {3}");

        #region Hashtag
        ///<summary>Hashtag {0} is system related cannot be selected.</summary>
        public static CustomError E5090 = CustomError.CreateNormalError("5090", @"Hashtag ‘{0}’ is system related cannot be selected.");
        ///<summary>Please use system related hashtag. (Not valid tag: {0})</summary>
        public static CustomError E5091 = CustomError.CreateNormalError("5091", @"Please use system related hashtag. (Not valid tag: {0})");
        #endregion

        ///<summary>The length of G/L account should be 7</summary>
        public static CustomError E5098 = CustomError.CreateNormalError("5098", @"The length of G/L account should be 7");

        ///<summary>G/L Account only can input numeric number.</summary>
        public static CustomError E5099 = CustomError.CreateNormalError("5099", @"G/L Account only can input numeric number.");

        ///<summary>Line No. {0} | Item ({1}) is planned item from WO. Please adopt from planned service list.</summary>
        public static CustomError E5093 = CustomError.CreateNormalError("5093", @"Line No. {0} | Item ({1}) is planned item from WO. Please adopt from planned service list.");
        #endregion


        public static CustomError E6027 = CustomError.CreateNormalError("6027", @"Invalid Equipment status. Equipment No.: {0}, Status: {1}");
        public static CustomError E6009 = CustomError.CreateNormalError("6009", @"{0} is in invalid status for {1}. Material Status: {2}, Equipment Status: {3}");

        /// <summary>Not Implemented.</summary>
        public static CustomError E9000 = CustomError.CreateNormalError("9000", @"Not Implemented.");
        /// <summary>Mock error. Remarks: {0}</summary>
        public static CustomError E9998 = CustomError.CreateNormalError("9998", @"Mock error. Remarks: {0}");
        /// <summary>Mock error. Remarks: {0}</summary>
        public static CustomError X9998 = CustomError.CreateSystemError("9998", @"Mock error. Remarks: {0}");
        /// <summary>Mock error.</summary>
        public static CustomError E9999 = CustomError.CreateNormalError("9999", @"Mock error.");
        /// <summary>Mock error.</summary>
        public static CustomError X9999 = CustomError.CreateSystemError("9999", @"Mock error.");
        /// <summary>Reserved for Information Logging.</summary>
        public static int I2990 = 2990;

        public static CustomError E9001 = CustomError.CreateNormalError("9001", @"{0}");

        private static List<CustomError> _customErrors;
        public static List<CustomError> GetAll() {
            if (_customErrors == null) {
                _customErrors = typeof(ErrorRegistry).GetFields().Select(o => o.GetValue(null)).OfType<CustomError>().ToList();
#if DEBUG
                foreach (CustomError error in _customErrors) {
                    if (_customErrors.Count(o => o.Code == error.Code && o.ErrorType == error.ErrorType) > 1) {
                        throw new Exception("Duplicate error code - Type: " + error.ErrorType + ", Code: " + error.Code);
                    }
                }
#endif
            }
            return _customErrors;
        }

        public static CustomError GetError(string errorCode) {
            if (errorCode[0].Equals('X')) {
                return GetAllSystemError().Single(o => o.Code == errorCode.Substring(1));
            } else if (errorCode[0].Equals('E')) {
                return GetAllNormalError().Single(o => o.Code == errorCode.Substring(1));
            } else {
                return GetAllNormalError().Single(o => o.Code == errorCode);
            }
        }

        public static Boolean IsMatch(IEnumerable<CustomError> expectedErrors, IEnumerable<CustomError> actualErrors) {
            if (expectedErrors.Count() != actualErrors.Count()) {
                return false;
            }

            for (int i = 0; i < expectedErrors.Count(); i++) {
                if (!IsMatch(expectedErrors.ElementAt(i), actualErrors.ElementAt(i))) {
                    return false;
                }
            }
            return true;

        }

        public static Boolean IsMatch(CustomError expectedError, CustomError actualError) {
            return IsMatch(expectedError, actualError, false);
        }

        public static Boolean IsMatch(CustomError expectedError, CustomError actualError, bool ignoreMessage) {
            if (expectedError.Code != actualError.Code) {
                return false;
            }

            if (expectedError.ErrorType != actualError.ErrorType) {
                return false;
            }

            return ignoreMessage || expectedError.Message == actualError.Message;
        }
        public static Boolean IsMatch(String errorCodes, String errorMessages, IEnumerable<CustomError> actualErrors) {
            return IsMatch(ConstructListError(errorCodes, errorMessages), actualErrors);
        }
        public static Boolean IsMatch(String errorCodes, String errorMessages, CustomError actualError) {
            return IsMatch(ConstructError(errorCodes, errorMessages), actualError);
        }

        public static Boolean IsMatch(String errorCodes, String errorMessages, CustomError actualError, bool ignoreMessage) {
            return IsMatch(ConstructError(errorCodes, errorMessages), actualError, ignoreMessage);
        }
        public static List<CustomError> ConstructListError(String errorCodes, String errorMessages) {
            IEnumerable<String> errorCode = errorCodes.Split(';');
            IEnumerable<String> errorMessage = errorMessages.Split(';');
            List<CustomError> listCustomError = new List<CustomError>();

            for (int i = 0; i < errorCode.Count(); i++) {
                listCustomError.Add(ConstructError(errorCode.ElementAt(i), errorMessage.ElementAt(i)));
            }

            return listCustomError;
        }

        public static CustomError ConstructError(String errorCode, String errorMessage) {
            char separtor = ',';
            if (errorMessage.Contains('~')) {
                separtor = '~';
            }

            String[] errorMessages = errorMessage.Split(separtor);
            return new CustomException(GetError(errorCode), errorMessages).CustomError;
        }

        public static List<CustomError> GetAllNormalError() {
            return GetAll().Where(o => o.ErrorType == ErrorType.Normal).ToList();
        }

        public static List<CustomError> GetAllSystemError() {
            return GetAll().Where(o => o.ErrorType == ErrorType.System).ToList();
        }

        public static List<CustomError> GetByMessage(string message) {
            if (String.IsNullOrEmpty(message)) {
                return GetAll();
            }
            string UpperMessage = message.ToUpper();
            return GetAll().Where(o => o.Message.ToUpper().Contains(UpperMessage)).ToList();
        }
    }
}
