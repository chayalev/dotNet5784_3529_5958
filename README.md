# EngineerTaskManagement

## Overview  
EngineerTaskManagement is a C#-based system designed to streamline task and engineer management. The system provides functionality for managers to create tasks, define dependencies, assign engineers, and generate schedules. Engineers can also log in to claim tasks that match their skill level and availability.

## Features  
### For Managers:  
- **Task Management**: Create tasks with defined levels, dependencies, and other attributes.  
- **Engineer Management**: Add and manage engineers with specific levels and expertise.  
- **Schedule Generation**: Automatically create a task schedule considering dependencies and start times.  
- **Task Assignment**: Assign tasks to engineers based on their skill level and other conditions.  

### For Engineers:  
- **Task Selection**: Engineers can log in, view available tasks, and claim tasks suitable for their level.  

## Technology Stack  
- **Language**: C#  
- **Frameworks**: WPF (Windows Presentation Foundation) for the user interface  
- **Data**: XML for data storage and processing  
- **Architecture**: Three-layer architecture, separating UI, business logic, and data access  
