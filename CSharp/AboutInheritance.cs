using Xunit;

namespace DotNetKoans.CSharp
{
    public class AboutInheritance : Koan
    {
        public class Dog
        {
            public string Name { get; set; }

            public Dog(string name)
            {
                Name = name;
            }

            // For a method/function to be overidden by sub-classes, it must be virtual.
            public virtual string Bark()
            {
                return "WOOF";
            }
        }

        public class Chihuahua : Dog
        {
            // The only way to "construct" a Dog is to give it a name. Since a 
            // Chihuahua 'is a Dog' it must conform to a public/protected
            // constructor. Since the only public/protected constructor for a 
            // dog requires a name, a public/protected constructor must also
            // require a Name.

            // # this is a constructor with the same signature as its superclass constructor.
            // # so we can access the superclass constructor (and we pass in an argument here) with keyword "base".

            // # this was interesting: http://stackoverflow.com/questions/3146152/c-if-a-class-has-two-constructors-what-is-the-best-way-for-these-constructors
            // # gist is that if you have multiple constructors (with different signatures or else it wouldn't work, I assume),
            // one can call the other by using :this plus arguments, or no arguments as is applicable.

            public Chihuahua(string name) : base(name)
            {
            }

            //Unless it doesn't. You have to call the base constructor at some point
            //with a name, but you don't have to have your class conform to that spec:

            // # I guess that if you don't specify a particular method within base (as in below),
            // # it means to call the method on base with the same signature (here, constructor with no arguments).
            public Chihuahua() : base("Ima Chihuahua")
            {
            }

            // For a Chihuahua to do something different than a regular "Dog"
            // when called to "Bark", the base class must be virtual and the
            // derived class must declare it as "override".
            public override string Bark()
            {
                return "yip";
            }

            // A derived class can have have methods/functions or properties
            // that are new behaviors altogether.
            public string Wag()
            {
                return "Happy";
            }
        }

        [Koan(1)]
        // # IsAssignableFrom: true if c and the current Type represent the same type, or if the current Type is in the inheritance hierarchy of c, or if the current Type is an interface that c implements, or if c is a generic type parameter and the current Type represents one of the constraints of c. false if none of these conditions are true, or if c is null.
        public void SubclassesHaveTheParentAsAnAncestor()
        {
            Assert.True(typeof(Dog).IsAssignableFrom(typeof(Chihuahua)));
        }

        [Koan(2)]
        public void AllClassesUltimatelyInheritFromAnObject()
        {
            Assert.True(typeof(System.Object).IsAssignableFrom(typeof(Chihuahua)));
        }

        [Koan(3)]
        public void SubclassesInheritBehaviorFromParentClass()
        {
            var chico = new Chihuahua("Chico");
            Assert.Equal("Chico", chico.Name);
        }

        [Koan(4)]
        public void SubclassesAddNewBehavior()
        {
            var chico = new Chihuahua("Chico");
            Assert.Equal("Happy", chico.Wag());

            //We can search the public methods of an object 
            //instance like this:
            Assert.NotNull(chico.GetType().GetMethod("Wag"));

            //So we can show that the Wag method isn't on Dog. 
            //Proving you can't wag the dog. 
            var dog = new Dog("Fluffy");
            Assert.Null(dog.GetType().GetMethod("Wag"));
        }

        [Koan(5)]
        public void SubclassesCanModifyExistingBehavior()
        {
            var chico = new Chihuahua("Chico");
            Assert.Equal("yip", chico.Bark());

            //Note that even if we cast the object back to a dog
            //we still get the Chihuahua's behavior. It truly
            //"is-a" Chihuahua
            Dog dog = chico as Dog;
            Assert.Equal("yip", dog.Bark());

            var fido = new Dog("Fido");
            Assert.Equal("WOOF", fido.Bark());
        }

        public class ReallyYippyChihuahua : Chihuahua
        {
            public ReallyYippyChihuahua(string name) : base(name) { }

            //It is possible to redefine behavior for classes where
            //the methods were not marked virtual - but it can really
            //get you if you aren't careful. For example:

            public new string Wag()
            {
                return "WAG WAG WAG!!";
            }

        }

        [Koan(6)]
        public void SubclassesCanRedefineBehaviorThatIsNotVirtual()
        {
            ReallyYippyChihuahua suzie = new ReallyYippyChihuahua("Suzie");
            Assert.Equal("WAG WAG WAG!!", suzie.Wag());
        }

        [Koan(7)]
        public void NewingAMethodDoesNotChangeTheBaseBehavior()
        {
            //This is vital to understand. In Koan 6, you saw that the Wag
            //method did what we defined in our class. But what happens
            //when we do this?
            Chihuahua bennie = new ReallyYippyChihuahua("Bennie");
            Assert.Equal("Happy", bennie.Wag());

            // # so I guess that if the WAGWAGWAG wag method had been an override method,
            // # it would have stuck with the reallyYippyChihuahua even after
            // # it'd been downcast to a chihuahua. Since it was "born" (instantiated) with it?

            //That's right. The behavior of the object is dependent solely
            //on who you are pretending to be. Unlike when you override a
            //virtual method. Remember this in your path to enlightenment.

        }

        public class BullDog : Dog
        {
            public BullDog(string name) : base(name) { }
            public override string Bark()
            {
                return base.Bark() + ", GROWL";
            }
        }

        [Koan(8)]
        public void SubclassesCanInvokeParentBehaviorUsingBase()
        {
            var ralph = new BullDog("Ralph");
            Assert.Equal("WOOF, GROWL", ralph.Bark());
        }

        public class GreatDane : Dog
        {
            public GreatDane(string name) : base(name) { }
            public string Growl()
            {
                return base.Bark() + ", GROWL";
            }
        }

        [Koan(9)]
        public void YouCanCallBaseEvenFromOtherMethods()
        {
            var george = new GreatDane("George");
            Assert.Equal("WOOF, GROWL", george.Growl());
        }
    }
}