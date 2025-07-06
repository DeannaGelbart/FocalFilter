**2025 Note: This project was last updated in 2016. Since the code is a decade old, a number of things would be changed if development was resumed today.**

# FocalFilter

FocalFilter is a free Windows productivity tool that helps you focus by temporarily blocking distracting websites. After the block timer runs out, your websites are available for you to view again.  For more information on how to use it, see the [website](https://focalfilter.com). 

This Github repo contains the Visual Studio project for FocalFilter. There are three parts:

FocalFilter/FocalFilter is the main app, which adds and removes blocks to the Windows hosts file under the user's control:

<img width="352" alt="Screenshot 2025-07-05 at 7 22 37 PM" src="https://github.com/user-attachments/assets/4dea32c0-dcc0-433e-9f5f-81e2904d19a2" />
<img width="429" alt="Screenshot 2025-07-05 at 7 22 30 PM" src="https://github.com/user-attachments/assets/c0a15a26-cf4a-47d0-9748-fa291f004c85" />

FocalFilter/FocalFilterHelper is a helper app which runs when Windows boots. It removes any blocks that FocalFilter previously put in the hosts file.  The idea is that if the user really needs to access a blocked site, they simply need to reboot.

Setup/ is the InstallShield configuration for installation.
