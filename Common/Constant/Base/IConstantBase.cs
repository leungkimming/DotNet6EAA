using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public interface IConstantBase {
        string Code { get; }
        string Description { get; }
    }
    public interface IConstantBaseWithChineseDescriptions {
        string ChsDescription { get; }
        string ChtDescription { get; }
    }
}
