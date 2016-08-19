# DiceBot

### About

Windows-only Skype bot for rolling dice using the old Desktop API, written in C# using the Skype4COM wrapper library.

### Features

* Roll any dice you can imagine, from 1d2 to 1000d100. (Hopefully you needn't imagine more than that.)
* Roll any group of dice you can imagine at once. (ex. 2d10+5+4d6-2d4)
* Command support. Message the bot with !help once it's running to see the basic ones.
 * Search blindly (or look at the source) to find secret ones.
* Statistics are optionally kept for all users.
 * When your (fellow) players complain about the dice treating them unfairly, shut them up conveniently with hard facts (or let them weep in dispair if they're right).
* Plugin support using [MEF](https://msdn.microsoft.com/en-us/library/dd460648(v=vs.110).aspx "Managed Extensibility Framework").

### System Requirements

* Windows XP or later (or equivalent server release)
* .NET Framework 4.0
* The oldest Skype release you can find is recommended as later ones increase memory usage and add no useful features.
 * v6.21 works

### How to use

* A dedicated (virtual) machine and a separate Skype account for the bot are recommended.
* Start Skype.
* Start the bot.
* Allow the bot to access Skype when prompted.

The bot can be used by messaging it directly or by adding it to a P2P group chat. Starting with version 6.11, Skype creates cloud-based chats through the UI. Message the bot with !newgroup to create a new P2P chat. Your account will be given admin rights automatically.

### Licence

MIT. (See [LICENSE](LICENSE))
