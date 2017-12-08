namespace Genetic_Algorithm
{
    public class Genome
    {
        private float[] _genes;
        private float _fitness;

        public Genome(float[] genes)
        {
            _genes = genes;
        }

        public Genome(float[] genes, float fitness)
        {
            _genes = genes;
            _fitness = fitness;
        }

        public float[] Genes 
        {
            get{return _genes;}
            set{_genes = value;}
        }

        public float Fitness 
        {
            get{return _fitness;}
            set{_fitness = value;}
        }

        public void Copy(Genome original)
	    {
		    _genes = original._genes;
		    _fitness = original._fitness;
	    }
		
    }
}
