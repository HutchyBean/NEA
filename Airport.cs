namespace NEA
{
    struct Airport
    {
        public string code;
        public string fullName;
        public int disLpl;
        public int disBoh;

        public Airport(string code, string fullname, int disLpl, int disBOH)
        {
            this.code = code;
            this.fullName = fullname;
            this.disLpl = disLpl;
            this.disBoh = disBOH;
        }
    }
}