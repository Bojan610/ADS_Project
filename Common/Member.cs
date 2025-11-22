using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Member
    {
        private string firstname;
        private string lastname;
        private long jmbg;
        private List<string> books;
        private DateTime modifyTime;

        public Member(string firstname, string lastname, long jmbg)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.jmbg = jmbg;
            books = new List<string>();
            modifyTime = DateTime.Now;
        }

        public Member() { }

        [DataMember]
        public string Firstname { get => firstname; set => firstname = value; }

        [DataMember]
        public string Lastname { get => lastname; set => lastname = value; }

        [DataMember]
        public long Jmbg { get => jmbg; set => jmbg = value; }

        [DataMember]
        public List<string> Books { get => books; set => books = value; }

        [DataMember]
        public DateTime ModifyTime { get => modifyTime; set => modifyTime = value; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            int counter = 1;

            sb.AppendLine($"{Firstname} {Lastname}");
            sb.AppendLine($"JMBG: {Jmbg}");
            sb.AppendLine("Books:");

            if (books.Count > 0)
            {
                foreach (var book in books)
                {
                    sb.AppendLine($"   {counter}. {book}");
                    counter++;
                }
            }
            else
            {
                sb.AppendLine("The member does not contain any books");
            }
           

            return sb.ToString();
        }
    }
}
