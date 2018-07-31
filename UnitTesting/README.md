# Unit testing in MS Visual Studio
## Wat is unit testing?

Unit testing deelt een applicatie op in kleine blokjes code. Dit kan varieren van een methode tot een complete klasse. Voor elk blokje code wordt appart een test
geschreven die bepaald of het blokje code doet wat het moet doen.
Deze tests staan volledig los van de rest van de applicatie.  
Een goede Unit Test is **A TRIP**
* Automatic -> het aanroepen en het resultaat verwerken gebeurt automatisch
* Thorough -> de test houdt rekening met alle mogelijke uitkomsten
* Repeatable -> de test is herhaalbaar en levert altijd dezelfde resultaten. De test gebruikt geen oncontroleerbare parameters
* Independent -> elke test is geschreven om 1 specifieke functie of stukje code te testen. Elke test is zelfstandig en maakt geen gebruik van externe code of andere testen.
* Professional -> net zoals productie code moet een test goed geschreven en leesbaar zijn met duidelijke namen voor methodes en variabelen.
  
Een goede unit test is ook snel om uit te voeren. Elke test die langer duurt dan een halve seconde is een slechte test.
  
## Waarom is dit nuttig?
Unit testing heeft een aantal voordelen:
* het geeft betere resultaten dan manueel een applicatie te debuggen
* het gaat sneller
* het gaat automatisch
* de tests kunnen herhaald worden
* geeft inzicht in de werking van de applicatie
* voorkomt dat foutieve code van development naar productie gaat

Unit tests testen de logica van een applicatie. Naast Unit tests zijn er ook integratie tests en end-to-end (E2E) tests om andere delen van de applicatie te testen.

## Wat is Test Driven Development (TDD)
Test Driven Development of TDD wil zeggen dat de programmeurs eerst Unit Tests schrijven voor dat de applicatie code wordt geschreven.
Dit brengt een aantal voordelen met zich mee:
* de code die wordt geschreven is essentieel, m.a.w er is geen overbodige code aanwezig
* de code die wordt geschreven wordt altijd getest
* door eerst tests te schrijven moet er worden nagedacht over hoe de code wordt aangeroepen, dit verbetert het ontwerp van de applicatie

## Hoe schrijven we Unit Tests in MS Visual Studio
Om Unit tests te schrijven hebben we een framework nodig. Dit framework geeft ons een **Library** die ons de nodige code geeft om onze tests te schrijven
en een **Test Runner** die onze tests uitvoert en ons feedback geeft.

Er zijn een aantal verschillende populaire frameworks:
* NUnit
* MSTest (built in in MS Visual Studio)
* XUnit
* ReSharper (test runner)

Om onze tests te schrijven voegen we een nieuw project toe aan onze solution van het type ***Unit Test project*** en geven het een naam volgens de volgende conventie
`[project name].UnitTests`
  
We passen volgende conventies toe:
* Naam van te testen klasse -> `[Klasse naam]Tests`
* Naam van te testen methodes -> `[Methode naam]_[Scenario]_[ExpectedBehaviour]`

Voorbeeld:  
De klasse die we willen testen is `Reservation`, de methode die we willen testen is `CanBeCancelledBy()`, in het scenario dat we willen testen
is de gebruiker een administrator, het verwachte gedrag of *Expected behaviour* van deze methode in dit scenario is dat we `True` terug krijgen.
  
```cs
[TestClass]
public class ReservationTests {
  [TestMethod]
  public void CanBeCancelledBy_UserIsAdmin_ReturnsTrue()
  {
  }
}
```

## Wat is dependency injection en waarom is dit nuttig?
Dependency Injection wil zeggen dat we een object zijn *dependencies* **geven** in plaats van dat dit object zelf zijn *dependencies* **maakt**.
Dit kan gebeuren met behulp van een constructor (constructor injection), een setter (setter injection) of een gespecialiseerd framework zoals **Spring**.
  
Voorbeeld:  
In dit voorbeeld gaan we uit van een **object** met de volgende constructor

```cs
public MijnKlasse() {
mijnObject = Factory.getObject();
}
```

In dit geval heeft `mijnKlasse` een **dependency** namelijk `mijnObject`. In het geval dat we een Unit Test willen schrijven voor mijnKlasse moeten
we een *Mock Object* maken voor `mijnObject` en de `Factory.getObject()` aanroep opvangen. In dit geval is het gemakkelijker om Dependency Injection toe
te passen met behulp van de constructor.
  
```cs
public mijnKlasse(MijnObject mijnObject) {
this.mijnObject = mijnObject();
}
```

Nu kunnen we een *Mock Object* maken voor `mijnObject` en dit via de constructor doorgeven.

## Wat is reflection?
Reflectie is de mogelijkheid van een applicatie om *at runtime* zijn eigen gedrag en structuur te bekijken en eventueel aan te passen.
In C# betekent dit dat we dynamisch informatie over de geladen assemblies en de gedefiniëerde types in deze assemblies kunnen opvragen in de vorm van een "Type" object.
Deze "Type" objecten kunnen we gebruiken om dynamisch instanties te maken en methodes aan te roepen of *invoken*. Om reflectie te gebruiken in C# moeten we
de namespace `System.Reflection` toevoegen aan de applicatie.
  
In het verhaal van Unit Testing kunnen we **reflection** gebruiken om `private` methodes te testen.
Dit brengt een aantal voor- en nadelen met zich mee:
* private methoden kunnen complexe logica bevatten die we beter kunnen testen met directe toegang in plaats van via een publieke methode
* het princiepe van unit testing is om de smalste blokjes functionaliteit te testen
* private methoden zouden al getest moeten zijn door het aanroepen via publieke methoden tijdens het testen
* naargelang het *refactoring* van de code moet de test code ook *gerefactored* worden.

Voorbeeld:  
We willen de private methode `MyPrivateMethod` testen van `ClassLibrary1.MyObject`

```cs
private string MyPrivateMethod(string strInput, DateTime dt, double 
 dbl) 
{
    return this.Name + ": " + strInput + ", " + 
     dt.ToString() + ", " + dbl.ToString();
}
```
  
```cs
public static object RunStaticMethod(System.Type t, string strMethod, 
 object [] aobjParams) 
{
    BindingFlags eFlags = 
     BindingFlags.Static | BindingFlags.Public | 
     BindingFlags.NonPublic;
    return RunMethod(t, strMethod, 
     null, aobjParams, eFlags);
}
```
  
```cs
public static object RunInstanceMethod(System.Type t, string strMethod, 
 object objInstance, object [] aobjParams) 
{
    BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | 
     BindingFlags.NonPublic;
    return RunMethod(t, strMethod, 
     objInstance, aobjParams, eFlags);
}
```
  
```cs
private static object RunMethod(System.Type t, string 
 strMethod, object objInstance, object [] aobjParams, BindingFlags eFlags) 
{
    MethodInfo m;
    try 
    {
        m = t.GetMethod(strMethod, eFlags);
        if (m == null)
        {
             throw new ArgumentException("There is no method '" + 
              strMethod + "' for type '" + t.ToString() + "'.");
        }
                                
        object objRet = m.Invoke(objInstance, aobjParams);
        return objRet;
    }
    catch
    {
        throw;
    }
}
```
Test code
```cs
[Test] public void TestPrivateInstanceMethod()
{
    string strExpected = "MyName: Hello, 5/24/2004 12:00:00 AM, 2.1";
     
    ClassLibrary1.MyObject objInstance 
     = new MyObject("MyName");
    
    object obj = 
     UnitTestUtilities.Helper.RunInstanceMethod(
     typeof(ClassLibrary1.MyObject), "MyPrivateMethod",
     objInstance, new object[3] {"Hello", 
     new DateTime(2004,05,24), 2.1});
    
    string strActual = Convert.ToString(obj);
    
    Assert.AreEqual(strExpected,strActual);
}
```
[Source](https://www.codeproject.com/Articles/9715/How-to-Test-Private-and-Protected-methods-in-NET)


