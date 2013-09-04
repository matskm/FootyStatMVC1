FootyStatMVC1
=============

Project aims:

  - To design and implement a web application in C# ASP.NET to gain experience in a wide variety of techniques.

  - Techniques exposed to include:
    - MVC
    - XML & XML schema definition (XSD)
    - Design patterns:
        - Strategy
        - Command
        - Mediator
        - Factory Method
        - Inversion of Control
    - Javascript
    - JQuery
    - Ninject Dependency Injection

  - In addition to basic C# techniques, the following techniques were employed:
    - Lambda expressions
    - Generic Collections
    - Reflection
    - LINQ 

Project Platform:

 - Programming language: C# ASP.NET 4.5
 - IDE: Microsoft Visual Studio Express 2012 for Web
 - Project framework: Model-View-Controller version 4
 - Deployment platform: Appharbor.com (URL: http://testfootystatv2.apphb.com)

Project description:

  - Business Domain: Sports Statistice (Premier League Football)
  
  - Input data: Detailed player and team statistics from the Premier League on a "game-by-game" basis.
  
  - User interface:
    - Initial drill-down on season, team-name and player leading to one player selected.
    - A list of statistics presented for the player (e.g., goals and assists)
    - A "constraints" interface allowing the user to recalculate statistics based on constraining the data (specific Gameweeks, home/away).
    
Approach:

  - In order to provide a significant programming challenge, I opted not to use a database, but rather have the input as a flat xml file, and reproduce much of the functionality of a database in c# code.
  
  - Thus, the project approach "reinvents the wheel" and is not how one would do this in the real business world. But this is deliberate to provide a programming challenge.
  
  - The core data-structure is a simple table (List<String[]>) which can be iterated over very quickly.
  
  - The approach to processing the table is to gather together as many operations as possible (e.g., building an index, calculating a stat) and do them all together.
  
  - The core MVC approach is adopted, separating the View, Controller, and Model. The majority of the framework developed is in the Model, which reproduces certain core functions of a database.
  
Class-by-Class description:

SnapView
========

Description: Class defining the core data-structure

Responsibilities:
  - Define the core data-structure (List<String[]>)
  - Define the basic algorithm for iterating over the data-structure
  - Contains meta-data about the core data-structure.
  

SnapViewDirector (SVD)
======================

Description: 
  - Class defining the relationship between the SnapView and other Colleagues. 
  - Is the "Director" in the Mediator pattern.
  - Other Colleagues are Filters, Constraints, Stat-calculators, Indexes which have complex relationships with the SnapView and each other.
  - The SVD defines and maintains these relationships (for example, it decides which other colleagues should be included in an iteration over the SnapView)
  
Responsibilities:
  - To define and maintain the logical relationships between Colleagues.
  
Family of Filters and Actions
=============================

Description:
  - Filters allow the SnapView to be iterated, and a portion to be discarded.
  - Actions include Indexing, Constraint, Stat-calculation:
      - None of these alter the underlying SnapView so are treated separately to Filters.
      - Indexing: build an index of a column of the SnapView
      - Constraint: in the iteration over the SnapView, ignore rows which are outside the "constraint"
      - Stat-calculation: re-calculate various pre-determined statistics.
  - The Filters and Actions provide the basic functionality to manipulate the core data-structure.
  
Family of Commands and CommandStrategies
========================================

Description:
  - CommandInvoker is the Invoker in the Command pattern. SnapViewDirector is the Reciever.
  - CommandInvoker interacts with Commands, which are bundled together with other commands in CommandStrategies (macros).
  - Because of the nature of the project, a "deferred execution" mechanism is built into this implementation of the Command Pattern:
    - The invidual Commands define actions to be performed on the SnapView, but do not execute them.
    - A series of Commands can then be bundled together and "executed" upon the SnapView together by the Command Invoker (using the SVD mediator/reciever).
    
Responsibilities:
  - To provide a framework for defining complex operations to be performed on the SnapView (core data-structure)
  


Possible Future extensions to the project
=========================================
  
  - Implement the entire project using a database.
  
  - Implement a persistible constraint/filter definition (user can save searches).
  
  - Code a JSON flat-file input instead of XML
  
  - Code exceptions where indicated by "exception" comments.
