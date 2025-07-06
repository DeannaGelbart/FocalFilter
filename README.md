**2025 Note: This project was last updated in 2016. Since the code is a decade old, a number of things would be changed if development was resumed today.**

# FocalFilter

FocalFilter is a free Windows productivity tool that helps you focus by temporarily blocking distracting websites. After the block timer runs out, your websites are available for you to view again.  For more information on how to use it, see the [website](https://focalfilter.com).  

FocalFilter was created by Deanna Gelbart and Shan Naziripour.

![ff_edit2](https://github.com/user-attachments/assets/cc710e5a-2888-4b9b-8541-0282c5c82d32)

<img width="429" alt="Screenshot 2025-07-05 at 7 22 30 PM" src="https://github.com/user-attachments/assets/c0a15a26-cf4a-47d0-9748-fa291f004c85" />


## Source code

This Github repo contains the Visual Studio project for FocalFilter. There are three parts:

FocalFilter/FocalFilter is the main app, which the user uses to add and removes blocks manually.

FocalFilter/FocalFilterHelper is a helper app which runs when Windows boots. It removes any blocks that FocalFilter previously put in the hosts file.  The idea is that if the user really needs to access a blocked site, they simply need to reboot.

Setup/ is the InstallShield configuration for installation.

