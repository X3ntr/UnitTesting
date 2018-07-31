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

## Wat is dependency injection en waarom is dit nuttig?
Dependency Injection wil zeggen dat we een object zijn *dependencies* **geven** in plaats van dat dit object zelf zijn *dependencies* **maakt**.
Dit kan gebeuren met behulp van een constructor (constructor injection), een setter (setter injection) of een gespecialiseerd framework zoals **Spring**.
  
Voorbeeld:  
In dit voorbeeld gaan we uit van een **object** met de volgende constructor

``` public MijnKlasse() {
mijnObject = Factory.getObject();
} ```

In dit geval heeft `mijnKlasse` een **dependency** namelijk `mijnObject`. In het geval dat we een Unit Test willen schrijven voor mijnKlasse moeten
we een *Mock Object* maken voor `mijnObject` en de `Factory.getObject()` aanroep opvangen. In dit geval is het gemakkelijker om Dependency Injection toe
te passen met behulp van de constructor.
  
``` public mijnKlasse(MijnObject mijnObject) {
this.mijnObject = mijnObject();
} ```

Nu kunnen we een *Mock Object* maken voor `mijnObject` en dit via de constructor doorgeven.

## Wat is reflection?