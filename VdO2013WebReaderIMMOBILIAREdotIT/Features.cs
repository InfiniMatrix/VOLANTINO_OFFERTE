using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPExtensionMethods;
using VdO2013SRCore;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    public class ReaderFeatures : SiteReaderValueListBase<FeatureWrapper>
    {
        public const string KRiepilogo = @"{descrizione}";
        public const string KRiepilogoMap = @"descrizione";
        public const string KRiepilogoXPath = @"//*[@class='descrizione']";
        public const string KZona = @"{zona}";
        public const string KZonaMap = @"zona";
        public const string KZonaXPath = @"//*[@id='titolo_mappa']//*[@class='indirizzo_mappa zone']";
        public const string KTipologia = @"{tipologia}";
        public const string KTipologiaMap = @"tipologia";
        public const string KTipologiaXPath = null;
        public const string KNMq = @"{superficie}";
        public const string KNMqMap = @"nMq";
        public const string KNMqXPath = null;
        public const string KNLocali = @"{locali}";
        public const string KNLocaliMap = @"nLocali";
        public const string KNLocaliXPath = null;

        //public const string KPostoAuto = @"{box}";
        //public const string KPostoAutoMap = @"PostoAuto";
        //public const string KPostoAutoXPath = null;

        //public const string KStato = @"{stato}";
        //public const string KStatoMap = @"stato";
        //public const string KStatoXPath = null;

        //public const string KArredamento = @"{arredamento}";
        //public const string KArredamentoMap = @"arredamento";
        //public const string KArredamentoXPath = null;

        public const string KClasseEnergetica = @"{classe_energetica}";
        public const string KClasseEnergeticaMap = @"classe_energetica";
        public const string KClasseEnergeticaXPath = @"//*[@id='primaryContent']//*[@id='energetic']";
        public const string KPrezzo = @"{prezzo}";
        public const string KPrezzoMap = @"prezzo";
        public const string KPrezzoXPath = @"//*[@class='info_annuncio']/div[1]/strong";
        public const string KProposta = @"{contratto}";
        public const string KPropostaMap = @"proposta";
        public const string KPropostaXPath = null;

        public const string KRiferimento = @"{codice_annuncio}";
        public const string KRiferimentoMap = @"riferimento";
        public const string KRiferimentoXPath = null;

        protected override int Populate()
        {
            if (_List != null)
            {
                _List.Add(new FeatureWrapper()
                {
                    Name = KRiepilogo,
                    Mapping = KRiepilogoMap,
                    TypeName = typeof(string).ToString(),
                    Nullable = true,
                    MaxSize = 4096,
                    XPath = KRiepilogoXPath,
                    Actions = new FeatureWrapper.ActionsWrapper()
                    {
                        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                        { 
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\t", NewText = "" },
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\r", NewText = "" },
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\n", NewText = "" },
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "https://www.facebook.com/pages/ClodiaCase-Servizi-Immobiliari/228350483846521", NewText = "" }
                        }
                    }
                });
                _List.Add(new FeatureWrapper()
                {
                    Name = KZona,
                    Mapping = KZonaMap,
                    TypeName = typeof(string).ToString(),
                    Nullable = true,
                    MaxSize = 0255,
                    XPath = KZonaXPath,
                    Actions = new FeatureWrapper.ActionsWrapper()
                    {
                        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                        { 
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\t", NewText = "" },
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\r", NewText = "" },
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\n", NewText = "" },
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "Zona:", NewText = "" }
                        }
                    }
                });
                _List.Add(new FeatureWrapper()
                {
                    Name = KTipologia,
                    Mapping = KTipologiaMap,
                    TypeName = typeof(string).ToString(),
                    Nullable = true,
                    MaxSize = 0255,
                    XPath = KTipologiaXPath,
                    Actions = new FeatureWrapper.ActionsWrapper()
                    {
                        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                        { 
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = ":", NewText = "" }
                        }
                    }
                });
                _List.Add(new FeatureWrapper()
                {
                    Name = KNMq,
                    Mapping = KNMqMap,
                    TypeName = typeof(int).ToString(),
                    Nullable = true,
                    MaxSize = -001,
                    XPath = KNMqXPath,
                    Actions = new FeatureWrapper.ActionsWrapper()
                    {
                        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                        {
                            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "m²", NewText = "" }
                        }
                    }
                });
                _List.Add(new FeatureWrapper() { Name = KNLocali, Mapping = KNLocaliMap, TypeName = typeof(int).ToString(), Nullable = true, MaxSize = -001, XPath = KNLocaliXPath });
                //_List.Add(new FeatureWrapper()
                //{
                //    Name = KPostoAuto_______,
                //    Mapping = KPostoAutoMap_______,
                //    TypeName = typeof(string).ToString(),
                //    Nullable = true,
                //    MaxSize = -001,
                //    XPath = KPostoAutoXPath_______,
                //    Actions = new FeatureWrapper.ActionsWrapper()
                //    {
                //        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                //        {
                //            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "Value cannot be null.", NewText = "" },
                //            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "Parameter name: source", NewText = "" },
                //            new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\n", NewText = "" }
                //        }
                //    }
                //});

                //_List.Add(new FeatureWrapper() { Name = KStato___________, Mapping = KStatoMap___________, TypeName = typeof(string).ToString(), Nullable = true, MaxSize = 0255, XPath = KStatoXPath___________ });

                //_List.Add(new FeatureWrapper() { Name = KArredamento_____, Mapping = KArredamentoMap_____, TypeName = typeof(string).ToString(), Nullable = true, MaxSize = 2048, XPath = KArredamentoXPath_____ });

                _List.Add(new FeatureWrapper()
                {
                    Name = KClasseEnergetica,
                    Mapping = KClasseEnergeticaMap,
                    TypeName = typeof(string).ToString(),
                    Nullable = true,
                    MaxSize = 2048,
                    XPath = KClasseEnergeticaXPath,
                    Actions = new FeatureWrapper.ActionsWrapper()
                    {
                        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                    {
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "Classe energetica immobile", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "&lt;!-- popolare value class con noTabEnergetic in ogni caso di assenza classe energetica esplicita --&gt;", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "&lt;!-- popolare con messaggio in base a casisitica - solo in caso di assenza classe energetica (A+, A, B, etc. ) --&gt;", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "INDICE PRESTAZIONE ENERGETICA (IPE): Non indicato|La classe energetica del presente immobile non è stata indicata", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "\n", NewText = "" }
                    }
                    }
                });
                _List.Add(new FeatureWrapper()
                {
                    Name = KPrezzo,
                    Mapping = KPrezzoMap,
                    TypeName = typeof(double).ToString(),
                    Nullable = true,
                    MaxSize = -001,
                    XPath = KPrezzoXPath,
                    Actions = new FeatureWrapper.ActionsWrapper()
                    {
                        Replacements = new FeatureWrapper.ActionsWrapper.ReplacementWrapper[]
                    {
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "&amp;euro; ", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = "&euro; ", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = ".", NewText = "" },
                        new FeatureWrapper.ActionsWrapper.ReplacementWrapper() { Text = " ", NewText = "" }
                    }
                    }
                });
                _List.Add(new FeatureWrapper() { Name = KProposta, Mapping = KPropostaMap, TypeName = typeof(string).ToString(), Nullable = true, MaxSize = 0255, XPath = KPropostaXPath });
                _List.Add(new FeatureWrapper() { Name = KRiferimento, Mapping = KRiferimentoMap, TypeName = typeof(string).ToString(), Nullable = true, MaxSize = 0255, XPath = KRiferimentoXPath });
            }

            return _List.Count;
        }
    }
}
