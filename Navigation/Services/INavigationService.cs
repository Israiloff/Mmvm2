using System.Threading.Tasks;
using Israiloff.Cashbox.Component.Navigation.Delegates;
using Israiloff.Cashbox.Component.Navigation.Model;
using Israiloff.Cashbox.Component.Navigation.Model.Enums;
using Israiloff.Cashbox.Component.Navigation.Model.StructuralModels;

namespace Israiloff.Cashbox.Component.Navigation.Services
{
    /// <summary>
    /// Navigation engine for manage and control GUI's navigation 
    /// </summary>
    public interface INavigationService
    {
        #region Events

        /// <summary>
        /// Notifies subscribers about navigation process started
        /// </summary>
        event NavigationStartedEventHandler NavigationStarted;

        /// <summary>
        /// Notifies subscribers about page change process is about to start
        /// </summary>
        event BeforePageChangeEventHandler BeforeNavigation;

        /// <summary>
        /// Notifies subscribers about page change process is finished
        /// </summary>
        event AfterPageChangeEventHandler AfterNavigation;

        #endregion

        #region Methods

        INavigationNode CreateTree(string nodeName);

        void CreateBranch(BranchModel branchModel);

        void ChangeBranchState(BranchInfo branchInfo, ElementState state);

        void Backward(BranchInfo branchInfo);

        void Backward(BranchInfo branchInfo, uint backStepCount);

        void Backward(BranchInfo branchInfo, string nodeName);

        void Forward(NavigateArgs navigateArgs);

        bool ContainsNode(BranchInfo branchInfo, string nodeName);

        #endregion

        #region Async methods

        Task BackwardAsync(BranchInfo branchInfo);

        Task BackwardAsync(BranchInfo branchInfo, uint backStepCount);

        Task BackwardAsync(BranchInfo branchInfo, string nodeName);

        Task ForwardAsync(NavigateArgs navigateArgs);

        #endregion
    }
}