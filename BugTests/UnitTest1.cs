using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitialStateIsTriaging()
        {
            var bug = new Bug();
            Assert.AreEqual(State.Triaging, bug.CurrentState);
        }

        [TestMethod]
        public void AssignMovesToInProgress()
        {
            var bug = new Bug();
            bug.Assign();
            Assert.AreEqual(State.InProgress, bug.CurrentState);
        }

        [TestMethod]
        public void FixMovesToResolved()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            Assert.AreEqual(State.Resolved, bug.CurrentState);
        }

        [TestMethod]
        public void VerifyMovesToClosed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            bug.Verify();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void RejectMovesToClosed()
        {
            var bug = new Bug();
            bug.Reject();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void ReopenFromResolvedMovesToReopened()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void ReopenFromClosedMovesToReopened()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            bug.Verify();
            bug.Reopen();
            Assert.AreEqual(State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void ReturnToTriageFromReopened()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            bug.Reopen();
            bug.ReturnToTriage();
            Assert.AreEqual(State.Triaging, bug.CurrentState);
        }

        [TestMethod]
        public void DeferFromInProgressReturnsToTriaging()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Defer();
            Assert.AreEqual(State.Triaging, bug.CurrentState);
        }

        [TestMethod]
        public void NeedMoreInfoStaysInTriaging()
        {
            var bug = new Bug();
            bug.NeedMoreInfo();
            Assert.AreEqual(State.Triaging, bug.CurrentState);
        }

        [TestMethod]
        public void CloseFromReopenedMovesToClosed()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            bug.Reopen();
            bug.Close();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void FullCycleWithReopen()
        {
            var bug = new Bug();
            bug.Assign();
            bug.Fix();
            bug.Reopen();
            bug.ReturnToTriage();
            bug.Assign();
            bug.Fix();
            bug.Verify();
            Assert.AreEqual(State.Closed, bug.CurrentState);
        }
    }
}
