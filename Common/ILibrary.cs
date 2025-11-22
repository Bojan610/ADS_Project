using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ILibrary
    {
        [OperationContract]
        [FaultContract(typeof(LibraryException))]
        [FaultContract(typeof(SecurityException))]
        void AddMember(string token, Member member);

        [OperationContract]
        [FaultContract(typeof(LibraryException))]
        [FaultContract(typeof(SecurityException))]
        void ModifyMember(string token, Member member);

        [OperationContract]
        [FaultContract(typeof(LibraryException))]
        [FaultContract(typeof(SecurityException))]
        void RemoveMember(string token, long jmbg);

        [OperationContract]
        [FaultContract(typeof(LibraryException))]
        [FaultContract(typeof(SecurityException))]
        bool GetMember(string token, long jmbg, out Member member);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Member> GetAllMembers(string token);

        [OperationContract]
        [FaultContract(typeof(LibraryException))]
        [FaultContract(typeof(SecurityException))]
        bool AddBookToMember(string token, long jmbg, params string[] books);

        [OperationContract]
        [FaultContract(typeof(LibraryException))]
        [FaultContract(typeof(SecurityException))]
        bool RemoveBookFromMember(string token, long jmbg, int bookNum);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void WriteDatabase(string token, Dictionary<long, Member> members);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        Dictionary<long, Member> ReadDatabase(string token, DateTime time);
    }
}
