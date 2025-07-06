**2025 Note: This project was last updated in 2016. Since the code is a decade old, a number of things would be changed if development was resumed today.**

# FocalFilter

FocalFilter is a free Windows productivity tool that helps you focus by temporarily blocking distracting websites. After the block timer runs out, your websites are available for you to view again.  For more information on how to use it, see the [website](https://focalfilter.com).  

FocalFilter was created by Deanna Gelbart and Shan Naziripour.

![ff_block2](https://github.com/user-attachments/assets/5a7d2604-bc32-4bdb-ab88-0cbf895b4cf7)

## Source code

This Github repo contains C# source code for FocalFilter, taken from the Visual Studio project. 

FocalFilter/FocalFilter is the main app, which the user uses to add and removes blocks manually.

FocalFilter/FocalFilterHelper is a helper app which runs when Windows boots. It removes any blocks that FocalFilter previously put in the hosts file.  The intent is that if the user really needs to access a blocked site, they simply need to reboot.
