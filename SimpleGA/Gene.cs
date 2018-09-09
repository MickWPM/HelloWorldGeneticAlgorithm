namespace SimpleGA
{
    class Gene
    {
        static System.Random rng;
        public static int MaxGeneValue = 126; //'~'
        public static int MinGeneValue = 32; //' '
        public static int GeneLength = 12; //'Hello World!'

        public char[] genes;

        public Gene()
        {
            SetupGenes(new char[0]);
        }

        public Gene(char[] genes)
        {
            SetupGenes(genes);
        }

        void SetupGenes(char[] genes)
        {
            if (genes.Length > GeneLength)
            {
                char[] newGenes = new char[GeneLength];
                for (int i = 0; i < GeneLength; i++)
                {
                    newGenes[i] = genes[i];
                }
                genes = newGenes;
            }
            else
            {
                if (genes.Length < GeneLength)
                {
                    if (rng == null) rng = new System.Random();

                    char[] newGenes = new char[GeneLength];
                    for (int i = 0; i < GeneLength; i++)
                    {
                        if (i < genes.Length)
                        {
                            newGenes[i] = genes[i];
                        }
                        else
                        {
                            newGenes[i] = (char)rng.Next(MinGeneValue, MaxGeneValue + 1);
                        }
                    }
                    genes = newGenes;
                }
            }
            this.genes = genes;
        }

        public void Mutate(int index, int mutation)
        {
            int curVal = genes[index];
            curVal += mutation;
            if (curVal < MinGeneValue) curVal += (MaxGeneValue - MinGeneValue);
            if (curVal > MaxGeneValue) curVal -= (MaxGeneValue - MinGeneValue);

            if ((curVal <MinGeneValue) || (curVal > MaxGeneValue))
            {
                System.Console.WriteLine("ERROR IN MUTATION LOGIC: curVal = " + curVal);
                curVal = 50;
            }

            genes[index] = (char)curVal;
        }

        public override string ToString()
        {
            return new string(this.genes);
        }
    }
}
