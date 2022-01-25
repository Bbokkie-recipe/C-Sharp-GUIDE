using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//인터페이스란, 함수의 형태만 물려 줄 수 있는 문법이다.
//사용자 정의 자료형
//'포함'
interface QuestUnit
{
    //멤버 변수도 작성할 수 없다.
    //무조건 public
    void Talk(QuestUnit _QuestUnit);
    //인터페이스는 함수 구현을 강제한다.
}

//코드 재활용성을 향상시키기 위한 문법이 상속이다.
class FightUnit
{
    protected int LV;
    protected string Name;
    protected int AT;
    protected int HP;
    protected int MAXHP;
    public FightUnit() 
    {
    }
    public bool IsDeath()
    {
        return HP<=0;
    }
    //프로퍼티도 다형성 가능(virtual)
    public virtual int GetAT
    {
        get
        {
            if(999< AT)
            {
                Console.WriteLine("최대 수정치를 넘겼습니다.");
            }
            return AT; 
        }
        set { }
    }

    //동적바인딩
    //다형성
    public virtual int GetATT()
    {
        return GetAT;
    }
    public void SetName(string _Names)
    {
        Name = _Names;
    }
    public void Damage(FightUnit _OtherUnit)
    {
        Console.Write(Name);
        Console.Write("가 ");
        Console.Write(_OtherUnit.AT);
        Console.WriteLine("의 데미지를 입었습니다.");
        Console.ReadKey();
        HP -= _OtherUnit.GetAT;
    }
    public void StatusRender()
    {
        Console.Write(Name);
        Console.WriteLine("의 능력치 : ============================");
        Console.Write("공격력 : ");
        Console.WriteLine(AT);
        Console.Write("체력 : ");
        Console.Write(HP);
        Console.Write("/");
        Console.WriteLine(MAXHP);
        Console.WriteLine("============================");
    }
}
//클래스는 웬만하면 자기의 책임을 스스로 하게 만들자
//=캡슐화
class Player : FightUnit, QuestUnit
{
    public enum PLAYERJOB
    {
        NOVICE,
        KNIGHT,
        FIGHTER,
        BERSERKER,
        FIREMAGE,
    }
    public enum DMGTYPE
    {
        PYSMG,
        FIREDMG,
        ICEDMG
    }
    int ItemATT = 5;//아이템 공격력
    int AttDef = 5;//물리 방어
    int FireDef = 5;//불 방어
    int IceDef = 5;//물 방어
    //생성자
    //만들어질때 무조건 한번 실행되어지는 함수(작성하지 않아도)
    //자신 클래스의 메모리를 리턴해주는 함수
    public Player() 
    { 
        LV = 1;
        Name = "player";
        AT = 10;
        HP = 50;
        MAXHP = 100;
    }
    //생성자 오버로딩
    public Player(int _Hp)
    {
        LV = 1;
        Name = "player";
        AT = 10;
        HP = _Hp;
        MAXHP = 100;
    }

    //부모클래스의 GetATT()재구현
    public override int GetATT()
    {
        return AT + ItemATT;
    }
    public void Hurt(int _Damage)
    {
        HP -= _Damage;
    }
        public void Hurt(int  _Damage, DMGTYPE _Type)
    {
        switch (_Type)
        {
            case DMGTYPE.PYSMG:
                _Damage -= AttDef;
            break;
            case DMGTYPE.FIREDMG:
                _Damage -= FireDef;
                break;
            case DMGTYPE.ICEDMG:
                _Damage -= IceDef;
                break;
            default:
                break;
        }
        Hurt(_Damage);
    }
    public void PrintHp()
    {
        //객체의 멤버 변수와 관련된 코드를 2번 이상 칠려면
        //함수로 따로 만들어라.
        Console.WriteLine("");
        Console.Write("현재 플레이어의 HP는 : ");
        Console.Write(HP);
        Console.WriteLine("입니다.");
        Console.ReadKey();
    }

    public void MAXHeal()
    {
        //함수는 비대하지 않을수록 좋다
        //10줄 안팎으로!

        if(HP>= MAXHP)
        {
            Console.WriteLine("");
            Console.WriteLine("이미 풀피입니다!");
            Console.ReadKey();
        }
        else 
        {
            HP = MAXHP;
            PrintHp();
        }
        return;
    }

    //인터페이스가 강제하는 함수는 무조건 public 
    public void Talk(QuestUnit _QuestUnit)
    {

    }

}
class NPC : QuestUnit
{
    //인터페이스가 강제하는 함수는 무조건 public 
    public void Talk(QuestUnit _QuestUnit)
    {

    }
}

class Monster : FightUnit
{
    private int EXP = 1;
    public Monster(string _Names)
    {
        LV = 1;
        Name = _Names;
        AT = 10;
        HP = 50;
        MAXHP = 100;
    }
    public override int GetATT()
    {
        return AT + LV;
    }
}

//에러나 혹은 잘못된 선택에 관한 것도 만든다
enum STARTSELECT
{
    SELECTTOWN,
    SELECTBATTLE,
    NONESELECT,
}

namespace TextRPG
{
    class Program
    { 
        //시작한다
        //마을로 갈지 싸움터로 갈지
        static STARTSELECT StartSelect()
        {
            Console.Clear();
            Console.WriteLine("1. 마을");
            Console.WriteLine("2. 배틀");
            Console.WriteLine("어디로 갈까?");
            ConsoleKeyInfo CKI = Console.ReadKey();
            switch (CKI.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("마을로 이동합니다.");
                    return STARTSELECT.SELECTTOWN;
                    Console.ReadKey();
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("배틀을 시작합니다.");
                    return STARTSELECT.SELECTBATTLE;
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다.");
                    return STARTSELECT.NONESELECT;
                    Console.ReadKey();
                    break;
            } 
        }

        static STARTSELECT Town(Player player)
        {
            while(true)
            {
                Console.Clear();
                player.StatusRender();
                Console.WriteLine("마을에서 무슨 일을 하시겠습니까?");
                Console.WriteLine("1. 체력을 회복한다.");
                Console.WriteLine("2. 무기를 강화한다.");
                Console.WriteLine("3. 마을을 나간다.");

                //초반에 프로그래밍의 전부
                //객체(클래스)를 선언해야 할 때
                //함수의 분기
                //함수의 통합, 분리(합칠 때와 쪼갤 때)

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        player.MAXHeal();
                        break;
                    case ConsoleKey.D2:
                        break;
                    case ConsoleKey.D3:
                        return STARTSELECT.SELECTBATTLE;
                }
            }
        }
        static STARTSELECT Battle(Player player)
        {
            //Console.WriteLine("아직 개장하지 않았습니다.");
            //Console.ReadKey();
            Monster NewMonster = new Monster("오크");
            //한쪽이 죽으면 마을 자동 이송
            while(false == player.IsDeath() && false == NewMonster.IsDeath())
            {
                Console.Clear();
                player.StatusRender();
                NewMonster.StatusRender();
                //업캐스팅 : 자식이 부모형이 되는 것
                //그럼으로서 자식의 능력은 버린다

                NewMonster.Damage(player);
                if(false == NewMonster.IsDeath())
                {
                    player.Damage(NewMonster);
                }
            }
            Console.WriteLine("싸움이 결판났습니다.");
            if(true == NewMonster.IsDeath())
            {
                Console.WriteLine("플레이어의 승리입니다.");
            }
            else
            {
                Console.WriteLine("몬스터의 승리입니다.");
            }
            Console.ReadKey();
            return STARTSELECT.SELECTTOWN;
        }
        static void Main(string[] args)
        {
            Player NewPlayer = new Player();
            STARTSELECT SelcectCheck = STARTSELECT.NONESELECT;
            while (true)
            {
                //함수 자체의 용도를 생각해라
                //정말 한가지의 용도로만 사용할 수 있나?
                switch (SelcectCheck)
                {
                    case STARTSELECT.NONESELECT:
                        SelcectCheck = StartSelect();
                        break;
                    case STARTSELECT.SELECTTOWN:
                        SelcectCheck = Town(NewPlayer);
                        break;
                    case STARTSELECT.SELECTBATTLE:
                        SelcectCheck = Battle(NewPlayer);
                        break;
                }
            }
            ConsoleKeyInfo KeyInfo = Console.ReadKey();
        }
    }
}
