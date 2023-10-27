using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdhesivoDian.Lib
{
    public class SecurityCodeClass
    {
        private string BaseNumberConstants = "3;7;13;17;19;23;29;37;41;43;47;53;59;67;71;73;79;83";

        private List<int> ConstantsInputs { get; set; }

        private List<int> ValuesInputs { get; set; }

        private Dictionary<int, int> ConstantsEquivalent { get; set; }

        private void ConstructorSecurityCode()
        {
            this.ConstantsInputs = new List<int>();
            this.ValuesInputs = new List<int>();
            this.ConstantsEquivalent = new Dictionary<int, int>();

            //Se agregan como base los 18 primeros números primos
            string[] intConstants = this.BaseNumberConstants.Split(";");
            for (int item = 0; item < intConstants.Length; item++)
            {
                this.ConstantsInputs.Add(int.Parse(intConstants[item]));
                this.ValuesInputs.Add(0);
            }

            //Se crean las equivalencias
            ConstantsEquivalent.Add(0, 0);
            ConstantsEquivalent.Add(1, 1);
            ConstantsEquivalent.Add(2, 9);
            ConstantsEquivalent.Add(3, 8);
            ConstantsEquivalent.Add(4, 7);
            ConstantsEquivalent.Add(5, 6);
            ConstantsEquivalent.Add(6, 5);
            ConstantsEquivalent.Add(7, 4);
            ConstantsEquivalent.Add(8, 3);
            ConstantsEquivalent.Add(9, 2);
            ConstantsEquivalent.Add(10, 1);
        }

        public SecurityCodeClass()
        {
            ConstructorSecurityCode();
        }

        public SecurityCodeClass(string valueInputs) : base()
        {
            ConstructorSecurityCode();

            //Se agregan en las posiciones los números de base como value
            int posicion = 0;
            foreach (var word in valueInputs)
            {
                this.ValuesInputs[valueInputs.Length - posicion - 1] = int.Parse(word.ToString());
                posicion++;
            }
        }

        public int GetSecurityCode()
        {
            int resultadoOperacion = 0;

            for (int ponderacion = 0; ponderacion < this.ConstantsInputs.Count; ponderacion++)
            {
                resultadoOperacion += this.ConstantsInputs[ponderacion] * this.ValuesInputs[ponderacion];
            }

            return this.ConstantsEquivalent[resultadoOperacion % 11];
        }
    }
}
