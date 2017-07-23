# How to Contribute

SmashBloc is designed to make contribution easy and effective from a developers' standpoint, and at its core it thrives on the creativity of curious programmers. Thanks for your interest in joining the project!

### For Experienced Developers

SmashBloc could always use new features to enhance the state and action space available to those that would rather program agents. The best course of action if you want to get directly involved, though, is to contact us directly at <smashbloc@gmail.com>.

### For New Developers

SmashBloc was designed to accomodate programmers that may not know all the ins and outs of programming yet. C# is an ideal lanaguage to learn, and likewise Unity is an excellent development environment for beginners.

If you're not familiar with basic Unity, we highly recommend the [Tanks! tutorial](https://unity3d.com/learn/tutorials/projects/tanks-tutorial). That tutorial will provide a general overview of many important systems necessary to understand how SmashBloc functions. Unity has a suite of tutorials for all skill levels in addition to excellent API documentation.

Once you feel you're comfortable working in Unity, proceed to the next section.

## Getting Started

* Make sure you have a [GitHub account](https://github.com/signup/free)
* Submit an [issue](https://help.github.com/articles/creating-an-issue/) and provide a description of the bug or feature you're tackling.
  * If you're creating a feature, label your issue [F]. If you're fixing a bug, label it [B]. If you're not sure, use [O].
* [Fork the repository](https://help.github.com/articles/fork-a-repo/) and begin development on the fork.

If you contribute successfully to the project, we'll add you as a collaborator and you can skip the last step.

## Making Changes

* Create a branch off the branch you want to improve (usually master).
* Do not work directly on the master branch.
* Keep your commits small and focused.
* Check for unnecessary whitespace and leftover comments with `git diff --check` before committing.
* Make sure your commit messages follow [best practices](https://www.slideshare.net/TarinGamberini/commit-messages-goodpractices).

## Submitting Changes

* Push your changes to a branch in your fork of the repository.
* Submit a pull request to the main SmashBloc repository.
* Update your ticket to mark that you have submitted code and are ready for it to be reviewed (Status: Ready for Merge).
  * Include a link to the pull request in the description of the ticket or in a comment.
* We will review the pull request as soon as we can and will provide feedback in the case it can't be immediately merged.
* After feedback has been given we expect responses within two weeks. If we don't hear back from you, we will close the pull request.

## Creating AI

SmashBloc is an AI sandbox, and the system by which new AI can be created is very flexible. It's based on a Brain-Body model. Here's how it works.

* Every (1) second, the Body will provide Information to the Brain. 
* When information arrives, the Brain must make a decision about what Command to give back to the Body.
* Every (1) second, the Body will look to see if it has an available Command to follow.

Some terms:
* **Information:** An EnvironmentInfo object (SmashBloc/Assets/Scripts/Utility/EnvironmentInfo). This information relates to the state of the body—its health, how many enemies are nearby and where, etc.
* **Body:** The physical form of the agent. Takes in sensory info to pass to the Brain.
* **Brain:** The mind of the agent. It can't access the Body's state naturally and thus relies on information that the Body passes in.
* **Command:** An object that implements ICommand (SmashBloc/Assets/Scripts/AI/Commands). Commands are constructed in the Brain with relevant State that they use to execute specific functions in the Body.

EnvironmentInfo classes are independent. Body classes generally extend MobileUnit. Brain classes usually extend MobileAI. Command classes usually extent MobileCommand.

There are a few examples of implemented AIs already in the project. If you have any further questions, please [contact us directly](mailto:smashbloc@gmail.com).