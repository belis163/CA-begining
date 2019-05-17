using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumulacja
{
    class Grid
    {
        public Cell[,] Map;
        int Map_height, Map_width;
        int[] statesTable;
        public int numberOfStates;
        int maxOfLiczbaZarodkow;

        public Grid()
        {

        }

        public Grid(int Map_width, int Map_height, int numberOfStates)
        {
            this.numberOfStates = numberOfStates + 1;
            this.Map_width = Map_width;
            this.Map_height = Map_height;
            this.statesTable = new int[this.numberOfStates];
            

            this.Map = new Cell[Map_height, Map_width];

            for(int i=0; i< Map_height; i++)
            {
                for(int j=0; j< Map_width; j++)
                {
                    this.Map[i, j] = new Cell();
                }
            }

            for(int i=0; i< numberOfStates; i++)
            {
                this.statesTable[i] = 0;
            }
        }

        public Cell[,] getMap()
        {
            return Map;
        }

        public void changeMapState(int i, int j, int state)
        {
            this.Map[i, j].setCellState(state);
        }

        public void UpdateVectorPeriodicalVonNeuman()
        {
            int[] neighbourhood = new int[4];
            int[,] previousIteration = new int[Map_height, Map_width];
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    previousIteration[i, j] = this.Map[i, j].getCellState();
                }
            }


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if(Map[i,j].getCellState() == 0)
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {

                            neighbourhood[0] = previousIteration[Map_height - 1, 0];
                            neighbourhood[1] = previousIteration[0, Map_width - 1];
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {

                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[0, Map_width - 2];
                            neighbourhood[2] = previousIteration[0, 0];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, 0];
                        }

                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i, Map_width - 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                        }

                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j];
                            neighbourhood[1] = previousIteration[i, j - 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                        }

                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i, j - 1];
                            neighbourhood[2] = previousIteration[i, 0];
                            neighbourhood[3] = previousIteration[i + 1, j];
                        }

                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i, j - 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[0, j];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i, j - 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                        }
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < numberOfStates; l++)
                            {
                                if (neighbourhood[k] == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }
        }

        internal void UpdateVectorAbsorbingVonNeuman()
        {

            {
                int[] neighbourhood = new int[4];
                int[,] previousIteration = new int[Map_height, Map_width];
                for (int i = 0; i < Map_height; i++)
                {
                    for (int j = 0; j < Map_width; j++)
                    {
                        previousIteration[i, j] = this.Map[i, j].getCellState();
                    }
                }


                for (int i = 0; i < Map_height; i++)
                {
                    for (int j = 0; j < Map_width; j++)
                    {

                        if (Map[i, j].getCellState() == 0)
                        {

                            //winkle
                            if (j == 0 && i == 0)
                            {

                                neighbourhood[0] = 0;
                                neighbourhood[1] = 0;
                                neighbourhood[2] = previousIteration[0, 1];
                                neighbourhood[3] = previousIteration[1, 0];
                            }
                            else if (j == Map_width - 1 && i == 0)
                            {

                                neighbourhood[0] = 0;
                                neighbourhood[1] = previousIteration[0, Map_width - 2];
                                neighbourhood[2] = 0;
                                neighbourhood[3] = previousIteration[1, Map_width - 1];
                            }
                            else if (j == 0 && i == Map_height - 1)
                            {
                                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                                neighbourhood[1] = 0;
                                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                                neighbourhood[3] = 0;
                            }

                            else if (j == Map_width - 1 && i == Map_height - 1)
                            {
                                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                                neighbourhood[2] = 0;
                                neighbourhood[3] = 0;
                            }
                            //krawedzie lewa
                            else if (j == 0 && i != 0 && i != (Map_height - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = 0;
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j != 0 && i == 0 && j != (Map_width - 1))
                            {
                                neighbourhood[0] = 0;
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = 0;
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = 0;
                            }

                            //reszta
                            else
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }
                            //////////////////////////////////////////////////////

                            for (int k = 0; k < 4; k++)
                            {
                                for (int l = 1; l < numberOfStates; l++)
                                {
                                    if (neighbourhood[k] == l)
                                    {
                                        statesTable[l]++;
                                    }
                                }
                            }

                            maxOfLiczbaZarodkow = statesTable[0];
                            for (int k = 1; k < numberOfStates; k++)
                            {
                                if (statesTable[k] > maxOfLiczbaZarodkow)
                                {
                                    maxOfLiczbaZarodkow = k;
                                }
                            }

                            Map[i, j].setCellState(maxOfLiczbaZarodkow);
                            for (int k = 0; k < numberOfStates; k++)
                            {
                                statesTable[k] = 0;
                            }

                        }
                    }
                }
            }
        }
    }
}
