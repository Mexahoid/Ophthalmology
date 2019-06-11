namespace Ophthalmology.ConfigLogics.Serialization
{
    class EyeJson
    {
        public string[] Params { get; set; }
        public int[] ParamsValues { get; set; }
        public int[] Diags { get; set; }
        public string Path { get; set; }

        public double[] Xses { get; set; }
        public double[] Yses { get; set; }
        public string[] Texts { get; set; }
    }
}
