using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBiblioteka
    {
        [OperationContract]
        [FaultContract(typeof(BibliotekaIzuzetak))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void DodajClana(string token, Clan clan);

        [OperationContract]
        [FaultContract(typeof(BibliotekaIzuzetak))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void IzmeniClana(string token, Clan clan);

        [OperationContract]
        [FaultContract(typeof(BibliotekaIzuzetak))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void IzbrisiClana(string token, long jmbg);

        [OperationContract]
        [FaultContract(typeof(BibliotekaIzuzetak))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        bool DobaviClana(string token, long jmbg, out Clan clan);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        List<Clan> SviClanovi(string token);

        [OperationContract]
        [FaultContract(typeof(BibliotekaIzuzetak))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        bool DodajKnjiguClanu(string token, long jmbg, params string[] knjige);

        [OperationContract]
        [FaultContract(typeof(BibliotekaIzuzetak))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        bool ObrisiKnjiguClanu(string token, long jmbg, params string[] knjige);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void PosaljiBazu(string token, Dictionary<long, Clan> baza);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        Dictionary<long, Clan> PreuzmiBazu(string token, DateTime vreme);
    }
}
