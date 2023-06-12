using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public GridValue[,] Grid { get; }
        public Direction direction { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }
        public readonly LinkedList<Direction> dirChanges= new LinkedList<Direction>();
        private readonly LinkedList<Position> SnakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();
        public GameState(int rows, int col)
        {
            Rows = rows; Columns = col;
            Grid = new GridValue[rows, col];
            direction = Direction.Right;
            Addsnake();
            AddFood();
           
        }
        private void Addsnake()
        {
            var r = Rows / 2;
            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                SnakePositions.AddFirst(new Position(r,c));
            }
            
        }
        private IEnumerable<Position> ReturnIfEmpty()
        {
          //  var FoodPosition= new Random().Next();
          for (int r = 0;r < Rows; r++)
            {
                for (int c = 0;c < Columns; c++)
                {
                    if (Grid[r,c]== GridValue.Empty)
                    {
                        yield return new Position(r,c);
                       
                    }
                }
            }
        }
        private void AddFood()
        {
            
            var emptyPositions= new List<Position>(ReturnIfEmpty());
            
            if (emptyPositions.Count == 0)
            {
                return;
            }
            Position pos= emptyPositions[new Random().Next(emptyPositions.Count)];
            Grid[pos.Row,pos.Column]= GridValue.Food;
        }
        public Position LocateHead()
        {
            return SnakePositions.First.Value;
        }
        public Position LocateTail()
        {
            return SnakePositions.Last.Value;
        }
        public IEnumerable<Position> LocateEntireSnakeBody()
        {
            return SnakePositions;

        }
        private void addhead(Position pos)
        {
            SnakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }
        private void removeTail()
        {
            var tailPos = SnakePositions.Last.Value;
            Grid[tailPos.Row, tailPos.Column] = GridValue.Empty;
            SnakePositions.RemoveLast();


        }

        public void ChangeDirection(Direction dir)
        {
            
            //direction = dir;
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
                
            }



        }
        public bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count==2)
            {
                return false;
                
            }
            Direction lastDir = GetLastDriection();
            return lastDir != newDir && newDir != lastDir.ReturnOpposite();
        }
        public Direction GetLastDriection()
        {
            if (dirChanges.Count==0)
            {
                return direction;
            }
            return dirChanges.Last.Value;
        }
        private bool OutsideOrNot(Position pos)
        {
           return pos.Row<0||pos.Row>=Rows || pos.Column<0||pos.Column>=Columns;
           

        }
        private GridValue WillHit(Position pos)
        {
            if (OutsideOrNot(pos))
            {
                return  GridValue.Outside ;
            } 
            else if (pos==LocateTail())
            {
                return GridValue.Empty;
            }

            return Grid[pos.Row, pos.Column];
        }
        
        public void Move()
        {
            if (dirChanges.Count>0)
            {
                direction = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }
            Position NewHeadPosition= LocateHead().Translate(direction);
           GridValue hit= WillHit(NewHeadPosition);
            if (hit== GridValue.Outside|| hit==GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit==GridValue.Empty)
            {
                removeTail();
                addhead(NewHeadPosition);
            } else if (hit==GridValue.Food)
            {
                addhead(NewHeadPosition);
                Score++;
                AddFood();
            }
        }
    }
}
/*
  
 
 */
