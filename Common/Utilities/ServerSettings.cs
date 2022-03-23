using Common.Shared;
using System.Configuration;
using System.Linq;

namespace Common.Utilities {
    public static class ServerSettings {
        public static string SystemName {
            get {
                if (Environment != EnvironmentType.Development) {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(nameof(SystemName))) {
                        return ConfigurationManager.AppSettings[nameof(SystemName)];
                    }
                }

                return AppConstants.SYSTEM_NAME;
            }
        }

        public static string SystemCode {
            get {
                if (Environment != EnvironmentType.Development) {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(nameof(SystemCode))) {
                        return ConfigurationManager.AppSettings[nameof(SystemCode)];
                    }
                }

                return string.Empty;
            }
        }

        public static EnvironmentType Environment {
            get {
                if (ConfigurationManager.AppSettings.AllKeys.Contains(nameof(Environment))) {
                    switch (ConfigurationManager.AppSettings[nameof(Environment)].Trim().ToUpper()) {
                        case EnvironmentCode.Production:
                            return EnvironmentType.Production;
                        case EnvironmentCode.UAT:
                            return EnvironmentType.UAT;
                        case EnvironmentCode.Testing:
                            return EnvironmentType.Testing;
                        case EnvironmentCode.Development:
                            return EnvironmentType.Development;
                        case EnvironmentCode.DataLoading:
                            return EnvironmentType.DataLoading;
                        case EnvironmentCode.StoryTest:
                            return EnvironmentType.StoryTest;
                        default:
                            return EnvironmentType.Development;
                    }
                } else {
                    return EnvironmentType.Development;
                }
            }
        }


        public static string System_ConnectionString {
            get {
                if (Environment != EnvironmentType.Development) {
                    if (ConfigurationManager.ConnectionStrings[nameof(System_ConnectionString)] != null) {
                        return ConfigurationManager.ConnectionStrings[nameof(System_ConnectionString)].ConnectionString;
                    }
                }


                return @"Data Source=.\SQLEXPRESS;Initial Catalog=;Integrated Security=True";
            }
        }

        public static string EventLogSource {
            get {
                if (Environment != EnvironmentType.Development) {
                    if (ConfigurationManager.AppSettings[nameof(EventLogSource)] != null) {
                        return ConfigurationManager.AppSettings[nameof(EventLogSource)];
                    }
                }

                return "Application";
            }
        }

        public static string HKElectricLoginDomain {
            get {
                if (Environment != EnvironmentType.Development) {
                    if (ConfigurationManager.AppSettings[nameof(HKElectricLoginDomain)] != null) {
                        return ConfigurationManager.AppSettings[nameof(HKElectricLoginDomain)];
                    }
                }

                return "HEH";
            }
        }
    }
}

