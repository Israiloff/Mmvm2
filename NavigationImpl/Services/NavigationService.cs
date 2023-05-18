using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mmvm.Container.Attributes;
using Mmvm.Logger;
using Mmvm.Navigation.Delegates;
using Mmvm.Navigation.Error;
using Mmvm.Navigation.Model;
using Mmvm.Navigation.Model.Enums;
using Mmvm.Navigation.Model.EventArgs;
using Mmvm.Navigation.Model.StructuralModels;
using Mmvm.Navigation.Services;
using Mmvm.Navigation.Utilities;

namespace Mmvm.Navigation.Impl.Services
{
    [Service(Name = nameof(NavigationService), LifetimeScope = LifetimeScope.SingleInstance)]
    public class NavigationService : INavigationService
    {
        #region Constructors

        public NavigationService(ILogger logger, INavigationUtility utility)
        {
            Logger = logger;
            Utility = utility;
            NavigationTree = new NavigationTree();
        }

        #endregion

        #region Events

        public event NavigationStartedEventHandler NavigationStarted;

        public event BeforePageChangeEventHandler BeforeNavigation;

        public event AfterPageChangeEventHandler AfterNavigation;

        public event BranchDisabledEventHandler BranchDisabled;

        #endregion

        #region Private properties

        /// <summary>
        /// Dictionary for store registered navigation pages (used only for info purposes) where key is page's name
        /// </summary>

        private NavigationTree NavigationTree { get; set; }

        private ILogger Logger { get; }

        private INavigationUtility Utility { get; }

        #endregion

        #region Public methods

        public bool ContainsNode(BranchInfo branchInfo, string nodeName)
        {
            Logger.Debug("{0} started for node : {1}", nameof(ContainsNode), nodeName);
            return NavigationTree
                .RootNodes[branchInfo.ParentNodeName]
                .Branches[branchInfo.BranchName]
                .Nodes
                .Any(node => node.Name == nodeName);
        }

        #region Backward navigation group

        #region Synchronus

        public void Backward(BranchInfo branchInfo)
        {
            Logger.Debug("NavigateBack method started for branch : {0}, of root : {1}",
                branchInfo.BranchName, branchInfo.ParentNodeName);

            ValidateBackwardArgs(branchInfo, NavigationTree.RootNodes);
            var result = GetBackwardNavigationUnit(branchInfo, NavigationTree.RootNodes);

            Logger.Debug("Navigation unit successfully resolved");

            if (!IsBranchActive(branchInfo, NavigationTree))
            {
                BranchDisabledNotification(branchInfo);
                return;
            }

            ExecuteNavigationAction(result);
            Logger.Debug("NavigateBack method successfully ended");
        }

        public void Backward(BranchInfo branchInfo, uint backStepCount)
        {
            Logger.Debug("NavigateBack method started for branch : {0}, of root : {1}, and steps back count : {2}",
                branchInfo.BranchName, branchInfo.ParentNodeName, backStepCount);

            ValidateBackwardArgs(branchInfo, NavigationTree.RootNodes);
            ValidateBackStepsCount(backStepCount,
                NavigationTree.RootNodes[branchInfo.ParentNodeName].Branches[branchInfo.BranchName].Nodes.Count);

            if (!IsBranchActive(branchInfo, NavigationTree))
            {
                BranchDisabledNotification(branchInfo);
                return;
            }

            var result = backStepCount == 1
                ? GetBackwardNavigationUnit(branchInfo, NavigationTree.RootNodes)
                : MultiStepBackNavigation(branchInfo, backStepCount, NavigationTree.RootNodes);

            Logger.Debug("Navigation unit successfully resolved");

            ExecuteNavigationAction(result);

            Logger.Debug("NavigateBack succeed");
        }

        public void Backward(BranchInfo branchInfo, string nodeName)
        {
            Logger.Debug("NavigateBack method started for page : {0}, of tree : {1}",
                nodeName, branchInfo);

            ValidateBackwardArgs(branchInfo, NavigationTree.RootNodes);
            ValidateNavigationArgs(nameof(nodeName), nodeName);

            if (!IsBranchActive(branchInfo, NavigationTree))
            {
                BranchDisabledNotification(branchInfo);
                return;
            }

            NavigationUnit navigationUnit;

            do
            {
                navigationUnit = GetBackwardNavigationUnit(branchInfo, NavigationTree.RootNodes);
            } while (navigationUnit?.NavigatingNode != nodeName);

            Logger.Debug("Navigation unit successfully resolved");

            ExecuteNavigationAction(navigationUnit);

            Logger.Debug("NavigateBack succeed");
        }

        #endregion

        #region Async

        public async Task BackwardAsync(BranchInfo branchInfo)
        {
            Logger.Debug("BackwardAsync started");
            await Task.Run(() => Backward(branchInfo));
        }

        public async Task BackwardAsync(BranchInfo branchInfo, uint backStepCount)
        {
            Logger.Debug("BackwardAsync started");
            await Task.Run(() => Backward(branchInfo, backStepCount));
        }

        public async Task BackwardAsync(BranchInfo branchInfo, string nodeName)
        {
            Logger.Debug("BackwardAsync started");
            await Task.Run(() => Backward(branchInfo, nodeName));
        }

        #endregion

        #endregion

        #region Tree creation group

        public INavigationNode CreateTree(string nodeName)
        {
            Logger.Debug("CreateRootNode started for node : {0}", nodeName);

            ValidateNavigationArgs(nameof(nodeName), nodeName);
            if (NavigationTree.RootNodes.ContainsKey(nodeName)) NodeAlreadyExistError(nodeName);
            NavigationTree.RootNodes.Add(nodeName, new RootNode(nodeName));
            if (!Utility.TryGetPageObject(nodeName, out var navigatingPageVm))
            {
                NavigationTree.RootNodes.Remove(nodeName);
                NodeObjectGetError(nodeName);
            }

            return navigatingPageVm;
        }

        public void CreateBranch(BranchModel branchModel)
        {
            Logger.Debug("CreateBranch started with param : {0}", branchModel);

            ValidateBranchCreationModel(branchModel);

            if (!NavigationTree.RootNodes[branchModel.ParentNodeName].Branches.ContainsKey(branchModel.BranchName))
            {
                NavigationTree.RootNodes[branchModel.ParentNodeName].Branches.Add(branchModel.BranchName,
                    new Branch(branchModel.BranchName, branchModel.NavigationCallback));
            }

            Logger.Debug("Branch successfully created");
        }

        #endregion

        #region Forward navigation group

        #region Synchronus

        public void Forward(NavigateArgs navigateArgs)
        {
            Logger.Debug("NavigateTo method started with args : {0}", navigateArgs);

            NavigationStarted?.Invoke(this,
                new NavigationStartedEventArgs(navigateArgs.BranchInfo, navigateArgs.NodeName, Direction.Forward));

            ValidateNavigationArgs(nameof(navigateArgs), navigateArgs);
            ValidateNavigationArgs(nameof(navigateArgs.BranchInfo), navigateArgs.BranchInfo);
            ValidateNavigationArgs(nameof(navigateArgs.NodeName), navigateArgs.NodeName);
            ValidateNavigationArgs(nameof(navigateArgs.BranchInfo.ParentNodeName),
                navigateArgs.BranchInfo.ParentNodeName,
                NavigationTree.RootNodes);
            ValidateNavigationArgs(nameof(navigateArgs.BranchInfo.BranchName), navigateArgs.BranchInfo.BranchName,
                NavigationTree.RootNodes[navigateArgs.BranchInfo.ParentNodeName].Branches);

            if (!IsBranchActive(navigateArgs.BranchInfo, NavigationTree))
            {
                BranchDisabledNotification(navigateArgs.BranchInfo);
                return;
            }

            if (!Utility.TryGetPageObject(navigateArgs.NodeName, out var navigatingPageVm))
            {
                NodeObjectGetError(navigateArgs.NodeName);
            }

            var branch = NavigationTree
                .RootNodes[navigateArgs.BranchInfo.ParentNodeName]
                .Branches[navigateArgs.BranchInfo.BranchName];

            var navigationUnit = GetForwardNavigationUnit(navigateArgs, GetCurrentNodeName(branch),
                navigatingPageVm, branch);

            ExecuteNavigationAction(navigationUnit);
            branch.Nodes.Push(new Node(navigationUnit.NavigatingNode, navigatingPageVm));

            Logger.Debug("NavigateTo method successfully ended");
        }

        #endregion

        #region Async

        public async Task ForwardAsync(NavigateArgs navigateArgs)
        {
            Logger.Debug("BackwardAsync started");
            await Task.Run(() => Forward(navigateArgs));
        }

        #endregion

        #endregion

        #region Change state group

        public void ChangeBranchState(BranchInfo branchInfo, ElementState state)
        {
            Logger.Debug("ChangeBranchState started for branch : {0}, with state : {1:G}", branchInfo, state);

            ValidateNavigationArgs(nameof(branchInfo), branchInfo);

            ValidateNavigationArgs(nameof(branchInfo.ParentNodeName), branchInfo.ParentNodeName);
            ValidateNavigationArgs(nameof(branchInfo.BranchName), branchInfo.BranchName);

            ValidateNavigationArgs(nameof(branchInfo.ParentNodeName), branchInfo.ParentNodeName,
                NavigationTree.RootNodes);
            ValidateNavigationArgs(nameof(branchInfo.BranchName), branchInfo.BranchName,
                NavigationTree.RootNodes[branchInfo.ParentNodeName].Branches);

            Logger.Debug("Changing element\'s state");
            NavigationTree
                .RootNodes[branchInfo.ParentNodeName]
                .Branches[branchInfo.BranchName]
                .IsActive = state == ElementState.Active;
        }

        #endregion

        #endregion

        #region Private methods

        private void BranchDisabledNotification(ICloneable branchInfo)
        {
            Logger.Warn("BranchDisabledNotification started for branch : {0}", branchInfo);
            BranchDisabled?.Invoke(this, new BranchDisabledEventArgs(branchInfo.Clone() as BranchInfo));
        }

        private bool IsBranchActive(BranchInfo branchInfo, NavigationTree navigationTree)
        {
            Logger.Debug("IsRouteActive started");
            return navigationTree.RootNodes[branchInfo.ParentNodeName].Branches[branchInfo.BranchName].IsActive;
        }

        #region Tree creation group

        private void ValidateBranchCreationModel(BranchModel branchModel)
        {
            Logger.Trace("ValidateBranchCreationModel started for branch {0}", branchModel);
            ValidateNavigationArgs(nameof(branchModel), branchModel);
            ValidateNavigationArgs(branchModel.ParentNodeName, branchModel.BranchName);
            ValidateNavigationArgs(nameof(branchModel.ParentNodeName), branchModel.ParentNodeName,
                NavigationTree.RootNodes);
            ValidateNavigationArgs(nameof(branchModel.NavigationCallback), branchModel.NavigationCallback);
            ValidateNavigationArgs(nameof(branchModel.NavigationCallback),
                branchModel.NavigationCallback);
        }

        private void NodeAlreadyExistError(string nodeName)
        {
            var message = $"Root node with the same name is already defined : {nodeName}";
            Logger.Error(message);
            throw new NavigationException(message);
        }

        #endregion

        #region Forward navigation group

        private void NodeObjectGetError(string nodeName)
        {
            var message = $"Can\'t create node with name {nodeName}. Make sure that node had been registered";
            Logger.Error(message);
            throw new NavigationException(message);
        }

        private string GetCurrentNodeName(Branch branch)
        {
            Logger.Debug("GetSimplePageModel started");

            if (branch.Nodes.Count > 0)
            {
                Logger.Trace("Getting current node from selected branch");
                var nodeName = branch.Nodes.Peek();
                Logger.Trace("Node got : {0}", nodeName);
                return nodeName.Name;
            }

            Logger.Debug("Branch\'s nodes list is empty. Returning empty string");
            return string.Empty;
        }

        private NavigationUnit GetForwardNavigationUnit(NavigateArgs branchInfo, string currentPageModel,
            INavigationNode navigatingNodeModel, Branch branch)
        {
            Logger.Debug("GetForwardNavigationUnit started");
            return new NavigationUnit(branchInfo.BranchInfo, currentPageModel, branchInfo.NodeName,
                navigatingNodeModel, branch.Callback);
        }

        #endregion

        #region Backward navigation group

        private void ValidateBackwardArgs(BranchInfo branchInfo, IDictionary<string, RootNode> roots)
        {
            Logger.Trace("CommonArgsValidation started");
            ValidateNavigationArgs(nameof(branchInfo), branchInfo);
            ValidateNavigationArgs(nameof(branchInfo.ParentNodeName), branchInfo.ParentNodeName);
            ValidateNavigationArgs(nameof(branchInfo.BranchName), branchInfo.BranchName);
            ValidateNavigationArgs(nameof(branchInfo.ParentNodeName), branchInfo.ParentNodeName, roots);
            ValidateNavigationArgs(nameof(branchInfo.BranchName), branchInfo.BranchName,
                roots[branchInfo.ParentNodeName].Branches);
            ValidateStackSize(branchInfo, roots);
        }

        private NavigationUnit MultiStepBackNavigation(BranchInfo branchInfo, uint backStepCount,
            IDictionary<string, RootNode> roots)
        {
            Logger.Trace("MultiStepBackNavigation started");

            for (var i = 0; i < backStepCount - 1; i++)
            {
                GetBackwardNavigationUnit(branchInfo, roots);
            }

            return GetBackwardNavigationUnit(branchInfo, roots);
        }

        private void ExecuteNavigationAction(NavigationUnit navigationUnit)
        {
            Logger.Trace("ExecuteNavigationAction started");
            BeforeNavigationNotification(navigationUnit);
            navigationUnit.NavigationCallback(navigationUnit.NavigatingNodeModel);
            AfterNavigationNotification(navigationUnit);
            Logger.Trace("ExecuteNavigationAction successfully ended");
        }

        private void AfterNavigationNotification(NavigationUnit navigationUnit)
        {
            Logger.Trace("After navigation notification started");
            AfterNavigation?.Invoke(this,
                new AfterNavigationEventArgs(navigationUnit.NavigatingNode,
                    navigationUnit.BranchInfo));
        }

        private void BeforeNavigationNotification(NavigationUnit navigationUnit)
        {
            Logger.Trace("Before navigation notification started");
            BeforeNavigation?.Invoke(this, new BeforeNavigationEventArgs(
                navigationUnit.BranchInfo,
                navigationUnit.CurrentNode,
                navigationUnit.NavigatingNode));
        }

        private void ValidateBackStepsCount(uint backStepCount, int navigationStackSize)
        {
            Logger.Trace("CheckBackStepsCount started");

            if (backStepCount != 0 && navigationStackSize > backStepCount) return;

            var message = $"Backward navigation can\'t navigate back to {backStepCount} steps. " +
                          $"Navigation stack size is : {navigationStackSize}";
            Logger.Error(message);
            throw new NavigationException($"{message}. You are definitely doing something wrong");
        }

        private NavigationUnit GetBackwardNavigationUnit(BranchInfo branchInfo,
            IDictionary<string, RootNode> roots)
        {
            Logger.Debug("NavigateBackPrimitive started for tree : {0}", branchInfo);

            ValidateStackSize(branchInfo, roots);
            var branch = roots[branchInfo.ParentNodeName].Branches[branchInfo.BranchName];

            var currentElement = branch.Nodes.Pop();

            CheckAndRemoveRootTails(roots, currentElement);

            var navigatingElement = roots[branchInfo.ParentNodeName]
                .Branches[branchInfo.BranchName]
                .Nodes
                .Peek();

            Logger.Debug("Navigating element successfully got");

            return new NavigationUnit(branchInfo, currentElement.Name, navigatingElement.Name,
                navigatingElement.Value, branch.Callback);
        }

        private void ValidateStackSize(BranchInfo branchInfo, IDictionary<string, RootNode> roots)
        {
            Logger.Trace("CheckStackSize started for tree {0}", branchInfo);
            if (roots[branchInfo.ParentNodeName].Branches[branchInfo.BranchName].Nodes.Count > 1) return;
            var message = $"Navigation stack size for tree {branchInfo} is less than 2 " +
                          $"(actual value is {roots[branchInfo.ParentNodeName].Branches[branchInfo.BranchName].Nodes.Count})";
            Logger.Error(message);
            throw new NavigationException(message);
        }

        private void CheckAndRemoveRootTails(IDictionary<string, RootNode> roots, Node node)
        {
            Logger.Warn("CheckAndRemoveRootTails for node : {0}", node.Name);
            roots.Remove(node.Name);
        }

        #endregion

        #region Validation group

        private void ValidateNavigationArgs(string propertyName, string value)
        {
            Logger.Trace("ValidateNavigationArgs started for property with name : {0}, and value : {1}",
                propertyName, value);

            if (!string.IsNullOrWhiteSpace(value)) return;

            var message = $"Property \"{propertyName}\" is required and can't be null or empty";
            Logger.Error(message);
            throw new NavigationException(message);
        }

        private void ValidateNavigationArgs(string propertyName, object value)
        {
            Logger.Trace("ValidateNavigationArgs started for property with name : {0}, and value : {1}",
                propertyName, value);

            if (value != null) return;

            var message = $"Property \"{propertyName}\" is required and can't be null";
            Logger.Error(message);
            throw new NavigationException(message);
        }

        private void ValidateNavigationArgs<TValue>(string propertyName, string key,
            IDictionary<string, TValue> dictionary)
        {
            Logger.Trace("ValidateNavigationArgs started");
            if (dictionary.ContainsKey(key)) return;
            var message = $"Map does not contain the given key : {key}. The key is value of property : {propertyName}";
            Logger.Error(message);
            throw new NavigationException(message);
        }

        #endregion

        #endregion
    }
}