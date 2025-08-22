using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013RS
{
    public class TextRes : MPCommonRes.TextRes
    {
        public const String programInitialization = "Inizializzazione...";
        public const String programStartupParamsInitialization = "Inizializzazione parametri d'avvio...";
        public const String programStartupParamFormat = "Switch: {0}\tName: {1}\tValue: {2}";
        public const String programRegisterReaders = "Registrazione Reader...";
        public const String programRegisteredReaderCount = "{0}.RegisteredTypeCount={1}";
        public const String programRegisteredReaderFormat = "{0}.RegisteredType: {1}";
        //public const String programReaderInitialization = "Inizializzazione Reader...";
        //public const String programInitializationCompleted = "Inizializzazione completata";
        public const String programGuiStart = "Avvio interfaccia utente...";

        public const String doReaderChangeConfirm = "Cambiare il sistema di importazione?\n\nAttenzione: le modifiche non salvate andranno perse.";
        
        public const String pdfFileExtension = ".pdf";
        public const String pdfFileFilter = "File Portable Document Format (*.pdf)|*.pdf";
        public const String jpgFileExtension = ".jpg";
        public const String jpgFileFilter = "Immagine JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg";
        public const String pdfFileSaveTitleFormat = "Salvataggio di {0}...";
        public const String pdfFileSavedOpenFolder = "Aprire la cartella di output?";
        
        public const String doReaderExportToPdf = "Refresh";
        public const String doReaderExportToPdfVersionFormat = "Version={0};";
        public const String doReaderExportToPdfCaption = "Rileggi";
        public const String doReaderExportToPdfLayoutNotFound = "Impossibile trovare il layout richiesto";
        public const String doReaderExportToPdfNoRowsFound = "Impossibile trovare i dati richiesti";

        //public const String readerWaitForCompletion = "Attendere il completamento dell'operazione corrente";
        //public const String readerCodiceAgenziaChangedConfirm = "Confermare il nuovo codice?";

        //public const String readerJobNameFormat = "job{0}";
        //public const String readerJobDateFormat = "yyyyMMddHHmmss";

        //public const String readerDownloadConfirmReloadAll = "ATTENZIONE!!\n\nTUTTE LE OFFERTE VERRANNO SOSTITUITE SENZA ALCUNA DISTINZIONE.\n\nProcedere?";
        //public const String readerDownloadConfirmReloadUpdate = "Attenzione: tutte le offerte non bloccate verranno aggiornate.\nProcedere?";
        //public const String readerDownloadStep = "Scaricamento";
        //public const String readerDownloadInfo = "Inizio...";
        //public const String readerDownloadInfoWebSite = "Caricamento sito web...";
        //public const String readerDownloadInfoWebSiteDone = "Completato";
        //public const String readerDownloadInfoWebSiteSave = "Salvataggio sito web...";
        //public const String readerDownloadInfoWebSiteSaveDone = "Completato";
        //public const String readerDownloadProgressFormat = "{0}%\t({1})\t{2}";

        //public const String readerDownloadedInfoAgenziaFormat = "N° Agenzia:\t{0}";
        //public const String readerDownloadedInfoRagioneSocialeFormat = "Ragione sociale:\t{0}";
        //public const String readerDownloadedInfoTipologiaFormat = "Tipologia:\t{0}";
        //public const String readerDownloadedInfoElementiFormat = "Elementi:\t{0}";
        //public const String readerDownloadedInfoElementiScrittiFormat = "Elementi salvati:\t:{0}";
        //public const String readerDownloadedInfoCompleted = "Completato";
        //public const String readerDownloadedInfoCompletedErrors = "Completato con errori:";
        //public const String readerDownloadedInfoCompletedContinue = "Proseguire con la scrittura delle offerte?";

        //public const String readerFillDBStep = "Pulizia";
        //public const String readerFillDBInfo = "Inizio...";
        //public const String readerFillDBRowsAffectedFormat = "(Righe: {0})";
        //public const String readerFillDBDeletingFormat = "Svuotamento '{0}'";
        //public const String readerFillDBInsertStep = "Caricamento";
        //public const String readerFillDBInsertingFormat = "Inserimento '{0}'";
        //public const String readerFillDBInsertingOffertaFormat = "Inserimento proposta '{0}' numero {1}";
        //public const String readerFillDBInsertingImmagineFormat = "Inserimento immagine '{0}' numero {1}";
        //public const String readerFillDBProgressFormat = "{0}%\t({1})\t{2}";
        //public const String readerFilledDBInfoCompleted = "Completato";
        //public const String readerFilledDBInfoCompletedErrors = "Completato con errori:";

        //public const String readerFillXMLStep = "Carica intestazione";
        //public const String readerFillXMLInfo = "Inizio...";
        //public const String readerFillXMLInfoNoRows = "Nessuna riga trovata.";
        //public const String readerFillXMLReadOfferte = "Lettura offerte";
        //public const String readerFillXMLReadOffertaFormat = "Lettura offerta {0} di {1}";
        //public const String readerFillXMLSaveQRCodeErrorFormat = "Errore salvataggio QRCode: {0}";
        //public const String readerFillXMLSaveQRCodeErrorInteropFormat = "Errore nel salvataggio di '{0}' con il formato {1} (Codice: 0x{2:x16}).\n{3}";
        //public const String readerFillXMLInfoCompleted = "Completato";

        //public const String readerFillXMLProgressFormat = "{0}%\t({1})\t{2}";

        //public const String readerFilledXMLInfoCompleted = "Completato";
        //public const String readerFilledXMLInfoCompletedErrors = "Completato con errori:";

        //public const String readerWaitFormCaptionSavingData = "Salvataggio dati";

        //public const String readerReadListaStep = "Lettura";
        //public const String readerReadListaStepSetup = "Lettura intestazioni";
        //public const String readerReadListaUrlOpening = "Apertura URL '{0}'...";
        //public const String readerReadListaTipologiaTutte = "Tutte";
        //public const String readerReadListaProposteCountFormat = "Conteggio proposte: {0}";
        //public const String readerReadListaPropostaFormat = "Proposta nr {0}";

        //public const String readerReadListaItemStep = "Lettura proposta...";
        //public const String readerReadListaItemErrorFormat = "Pagina:{0}\nXPath:{1}";

        //public const String readerReadListaItemFeaturesFormat = "Dettaglio {0}";

        //public const String readerReadListaItemImagesDownload = "Scaricamento {0} {1}";
        //public const String readerReadListaItemImagesDownloadOK = "completato";
        //public const String readerReadListaItemImagesDownloadKO = "errore";

        //public const String readerSaveListaStep = "Salvataggio";
        //public const String readerSaveListaInfo = "Salvataggio proposte...";
        //public const String readerSaveListaStepSave = "Salvataggio intestazione...";
        //public const String readerSaveListaSaveWriteFormat = "Scrittura '{0}'={1}";
        //public const String readerSaveListaCodiceAgenzia = "Codice agenzia";
        //public const String readerSaveListaRagioneSociale = "Ragione sociale";
        //public const String readerSaveListaTipologia = "Tipologia proposta";
        //public const String readerSaveListaElementi = "Totale elementi";
        //public const String readerSaveListaItemFormat = "Scrittura proposta '{0}' nr. {1}";
        //public const String readerSaveListaItemNameFormat = "{0}{1}.{2}";
        //public const String readerSaveListaItemFeatureFormat = "Scrittura dettaglio '{0}'";
        //public const String readerSaveListaItemFeatureNameFormat = "{0}{1}.{2}{3}{4}";

        //public const String domSearchRootNodeFormat = "Root: {0}";
        //public const String domSearchNodeFormat = "Id:{0};\tLine:{1};\tClass:{2};\tAttributes:{3};\tInnerText:{4};";
        //public const String domSearchTagA = "a";
        //public const String domSearchTagAAttrHREF = "href";
        //public const String domSearchTagIMG = "img";
        //public const String domSearchTagIMGAttrSRC = "src";

        //public const String mainFormClosing = "Uscire dall'applicazione?";

        public struct htmlDecodePart
        {
            public String htmlText;
            public String plainText;
        }

        public static htmlDecodePart[] htmlDecodeParts = new htmlDecodePart[5]{
            new htmlDecodePart() { htmlText="&sup1", plainText="¹" },
            new htmlDecodePart() { htmlText="&sup2", plainText="²" },
            new htmlDecodePart() { htmlText="&sup3", plainText="³" },
            new htmlDecodePart() { htmlText="&amp;euro;", plainText="€" },
            new htmlDecodePart() { htmlText="&euro;", plainText="€" },
        };
        public const String htmlExceptionNoDocument = "Nessun documento caricato";
    }
}
