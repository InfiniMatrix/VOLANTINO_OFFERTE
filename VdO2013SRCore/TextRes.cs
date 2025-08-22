using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public class TextRes : VdO2013Core.TextRes
    {
        public const String tagOfferta = "Offerta";
        public const String tagDettaglio = "Dettaglio";
        public const String tagDettaglioB = tagDettaglio + "[";
        public const String tagDettaglioE = "]";

        public const String tagImmagine = "Immagine";
        public const String tagImmagineB = tagImmagine + "{";
        public const String tagImmagineE = "}";

        public const String tagFeatureSectionEx = "featuresSection";

        public const String tagFeatureSectionGroupName = "features";
        public const String tagFeatureSectionNameFormat = "feature{0}";

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
