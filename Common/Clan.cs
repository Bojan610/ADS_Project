using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Clan
    {
        private string ime;
        private string prezime;
        private long jmbg;
        private List<string> knjige;
        private DateTime vremeIzmene;

        public Clan(string ime, string prezime, long jmbg)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.jmbg = jmbg;
            knjige = new List<string>();
            vremeIzmene = DateTime.Now;
        }

        public Clan() { }

        [DataMember]
        public string Ime { get => ime; set => ime = value; }

        [DataMember]
        public string Prezime { get => prezime; set => prezime = value; }

        [DataMember]
        public long Jmbg { get => jmbg; set => jmbg = value; }

        [DataMember]
        public List<string> Knjige { get => knjige; set => knjige = value; }

        [DataMember]
        public DateTime VremeIzmene { get => vremeIzmene; set => vremeIzmene = value; }

        public override string ToString()
        {
            string ret = "";
            ret += $"{Ime} {Prezime}, {Jmbg}, lista knjiga:\n";
            foreach (string item in knjige)
            {
                ret += $"{item}\n";
            }

            return ret;
        }
    }
}
