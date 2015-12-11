using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewDurnit.Models;

namespace NewDurnit
{
    public class InitInstructionModel
    {
        public string Address { get; set; }
        public string Port { get; set; }
        public InitInstructions Instruction { get; set; }
        public string NameNodeAddress { get; set; }
        public string NameNodePort { get; set; }

        public List<DataNodeModel> dataNodes { get; set; }

        public InitInstructionModel()
        {
            this.Instruction = InitInstructions.NONE;
        }
    }
}
