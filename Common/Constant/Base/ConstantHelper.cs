using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public static class ConstantHelper {
        static Dictionary<string, Dictionary<string, IConstantBase>> allConstantBase = new Dictionary<string, Dictionary<string, IConstantBase>>();
        public static List<Type> TempType = new List<Type>();

        public static void Subscribe(IConstantBase constantBase) {
            string guid = constantBase.GetType().GUID.ToString();

            if (!allConstantBase.ContainsKey(guid)) {
                allConstantBase.Add(guid, new Dictionary<string, IConstantBase>());
            }

            //if (allConstantBase[guid].ContainsKey(constantBase.Code)) {
            //    throw new CustomException(ErrorRegistry.E2033, constantBase.GetType().ToString(), constantBase.Code ?? String.Empty);
            //}

            allConstantBase[guid].Add(constantBase.Code, constantBase);
        }

        public static IEnumerable<IConstantBase> GetAllConstants(Type type) {
            string guid = type.GUID.ToString();
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            return allConstantBase[guid].Values;
        }

        public static IEnumerable<T> GetAllConstants<T>() where T : IConstantBase {
            return GetAllConstants(typeof(T)).OfType<T>();
        }

        public static T GetConstantByCode<T>(string code) where T : IConstantBase {
            return GetAllConstants<T>().FirstOrDefault(o => o.Code == code);
        }

        public static T GetConstantByDescription<T>(string description) where T : IConstantBase {
            return GetAllConstants<T>().FirstOrDefault(o => o.Description == description);
        }

        public static string GetDescriptionByCode<T>(string code) where T : IConstantBase {
            if (!IsConstant<T>(code)) {
                return String.Empty;
            }

            return GetAllConstants<T>().First(o => o.Code == code).Description;
        }

        public static bool IsConstant<T>(string code) where T : IConstantBase {
            return GetAllConstants<T>().Any(o => o.Code == code);
        }
    }
}
