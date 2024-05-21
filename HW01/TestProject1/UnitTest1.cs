using HW01_2024;

namespace TestProject1
{
    public class Tests
    {
        Duel g = new Duel();
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FImonTest()
        {
            var fire1 = new FireFImon(5, 5, 5, "fast boi");
            var fire2 = new FireFImon(5, 10, 5, "fat boi");

            var leaf1 = new LeafFImon(5, 10, 1, "lose boi");
            var leaf2 = new LeafFImon(5, 11, 5, "win boi");
            var leaf3 = new LeafFImon(4, 11, 5, "draw boi");

            Assert.AreEqual(g.TimeBasedFImonDuel(fire1, leaf1), (0, 1)); // fire wins (crit)
            fire1.RestoreHp(); leaf1.RestoreHp();
            Assert.AreEqual(g.TimeBasedFImonDuel(fire1, leaf2), (1, 0)); // leaf wins 1hp clutch
            fire1.RestoreHp(); leaf2.RestoreHp();
            Assert.AreEqual(g.TimeBasedFImonDuel(fire1, leaf3), (1, 1)); // draw
            fire1.RestoreHp(); leaf3.RestoreHp();

            var water1 = new WaterFImon(5, 5, 6, "win water");
            var water2 = new WaterFImon(5, 5, 5, "draw (lose)");
            var water3 = new WaterFImon(4, 10, 4, "slow lose");
            var water4 = new WaterFImon(6, 10, 5, "draw water");

            Assert.AreEqual(g.TimeBasedFImonDuel(fire2, water1), (1, 0)); // water wins (faster attack)
            fire2.RestoreHp(); water1.RestoreHp();
            Assert.AreEqual(g.TimeBasedFImonDuel(fire2, water2), (1, 1)); // trade
            fire2.RestoreHp(); water2.RestoreHp();
            Assert.AreEqual(g.TimeBasedFImonDuel(fire2, water3), (0, 1)); // water loses slow attack speed
            fire2.RestoreHp(); water3.RestoreHp();

            Assert.AreEqual(g.TimeBasedFImonDuel(leaf2, water3), (0, 1)); // leaf wins (crit)
            leaf2.RestoreHp(); water3.RestoreHp();
            Assert.AreEqual(g.TimeBasedFImonDuel(leaf3, water4), (1, 1)); // trade
            leaf3.RestoreHp(); water4.RestoreHp();
            Assert.AreEqual(g.TimeBasedFImonDuel(leaf1, water1), (1, 0)); // water wins attack speed
            leaf1.RestoreHp(); water1.RestoreHp();
        }

        [Test]
        public void TrainerTest1()
        {
            Duel.timeBased = true;

            var t1 = new Trainer("t1", 0);
            var t2 = new Trainer("t2", 0);
            
            t1.PickCharacters([0,2,4]);
            t1.ChangePositions([1,0,2]);

            t2.PickCharacters([7,8,9]);

            Assert.AreEqual(Duel.TrainerDuel(t1, t2), 0); // from example
        }

        [Test]
        public void TrainerTest2()
        {
            Duel.timeBased = false;

            var t1 = new Trainer("t1", 0);
            var t2 = new Trainer("t2", 0);

            t1.PickCharacters([0, 2, 4]);
            t1.ChangePositions([1, 0, 2]);

            t2.PickCharacters([7, 8, 9]);

            Assert.AreEqual(Duel.TrainerDuel(t1, t2), 0); // from example
        }

        [Test]
        public void FImonTest2()
        {
            var fire1 = new FireFImon(5, 5, 5, "fast boi");
            var fire2 = new FireFImon(5, 10, 5, "fat boi");

            var leaf1 = new LeafFImon(5, 10, 1, "lose boi");
            var leaf2 = new LeafFImon(5, 11, 5, "win boi");
            var leaf3 = new LeafFImon(4, 11, 5, "draw boi");

            Assert.AreEqual(Duel.NormalFImonDuel(fire1, leaf1), (0, 1)); // fire wins (crit)
            fire1.RestoreHp(); leaf1.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(fire1, leaf2), (1, 0)); // leaf wins 1hp clutch
            fire1.RestoreHp(); leaf2.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(fire1, leaf3), (0, 1)); // fire wins
            fire1.RestoreHp(); leaf3.RestoreHp();

            var water1 = new WaterFImon(5, 5, 6, "win water");
            var water2 = new WaterFImon(5, 5, 5, "draw (lose)");
            var water3 = new WaterFImon(4, 10, 4, "slow lose");
            var water4 = new WaterFImon(6, 10, 5, "draw water");

            Assert.AreEqual(Duel.NormalFImonDuel(fire2, water1), (1, 0)); // water wins (faster attack)
            fire2.RestoreHp(); water1.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(fire2, water2), (0, 1)); // fire wins goes first
            fire2.RestoreHp(); water2.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(water2, fire2), (0, 1)); // water wins goes first
            fire2.RestoreHp(); water2.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(fire2, water3), (0, 1)); // water loses slow attack speed
            fire2.RestoreHp(); water3.RestoreHp();

            Assert.AreEqual(Duel.NormalFImonDuel(leaf2, water3), (0, 1)); // leaf wins (crit)
            leaf2.RestoreHp(); water3.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(leaf3, water4), (0, 1)); // leaf wins
            leaf3.RestoreHp(); water4.RestoreHp();
            Assert.AreEqual(Duel.NormalFImonDuel(leaf1, water1), (0, 1)); // water wins attack speed
            leaf1.RestoreHp(); water1.RestoreHp();
        }
    }
}