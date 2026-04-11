using System;
using Stateless;

namespace BugPro
{
    public enum State
    {
        Open,
        Triaging,
        InProgress,
        Resolved,
        Closed,
        Reopened
    }

    public enum Trigger
    {
        Submit,
        Assign,
        Defer,
        Reject,
        Fix,
        Verify,
        Reopen,
        Close,
        NeedMoreInfo,
        ReturnToTriage
    }

    public class Bug
    {
        private readonly StateMachine<State, Trigger> _machine;
        public State CurrentState => _machine.State;

        public Bug()
        {
            _machine = new StateMachine<State, Trigger>(
                State.Triaging);

            _machine.Configure(State.Triaging)
                .Permit(Trigger.Assign, State.InProgress)
                .Permit(Trigger.Reject, State.Closed)
                .Permit(Trigger.Defer, State.Triaging)
                .Permit(Trigger.NeedMoreInfo, State.Triaging)
                .OnEntry(() =>
                    Console.WriteLine("Состояние: Разбор дефектов"));

            _machine.Configure(State.InProgress)
                .Permit(Trigger.Fix, State.Resolved)
                .Permit(Trigger.Defer, State.Triaging)
                .Permit(Trigger.NeedMoreInfo, State.Triaging)
                .OnEntry(() =>
                    Console.WriteLine("Состояние: Исправление"));

            _machine.Configure(State.Resolved)
                .Permit(Trigger.Verify, State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .OnEntry(() =>
                    Console.WriteLine("Состояние: Проверка исправления"));

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened)
                .OnEntry(() =>
                    Console.WriteLine("Состояние: Закрытие"));

            _machine.Configure(State.Reopened)
                .Permit(Trigger.ReturnToTriage, State.Triaging)
                .Permit(Trigger.Close, State.Closed)
                .OnEntry(() =>
                    Console.WriteLine("Состояние: Переоткрытие"));
        }

        public void Assign() =>
            _machine.Fire(Trigger.Assign);
        public void Defer() =>
            _machine.Fire(Trigger.Defer);
        public void Reject() =>
            _machine.Fire(Trigger.Reject);
        public void Fix() =>
            _machine.Fire(Trigger.Fix);
        public void Verify() =>
            _machine.Fire(Trigger.Verify);
        public void Reopen() =>
            _machine.Fire(Trigger.Reopen);
        public void Close() =>
            _machine.Fire(Trigger.Close);
        public void NeedMoreInfo() =>
            _machine.Fire(Trigger.NeedMoreInfo);
        public void ReturnToTriage() =>
            _machine.Fire(Trigger.ReturnToTriage);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Сценарий 1: Успешное исправление ---");
            var bug1 = new Bug();
            bug1.Assign();
            bug1.Fix();
            bug1.Verify();
            Console.WriteLine($"Итог: {bug1.CurrentState}");

            Console.WriteLine("\n--- Сценарий 2: Отклонение бага ---");
            var bug2 = new Bug();
            bug2.Reject();
            Console.WriteLine($"Итог: {bug2.CurrentState}");

            Console.WriteLine("\n--- Сценарий 3: Переоткрытие ---");
            var bug3 = new Bug();
            bug3.Assign();
            bug3.Fix();
            bug3.Reopen();
            bug3.ReturnToTriage();
            bug3.Assign();
            bug3.Fix();
            bug3.Verify();
            Console.WriteLine($"Итог: {bug3.CurrentState}");

            Console.WriteLine("\n--- Сценарий 4: Нужно больше информации ---");
            var bug4 = new Bug();
            bug4.NeedMoreInfo();
            bug4.Assign();
            bug4.Fix();
            bug4.Verify();
            Console.WriteLine($"Итог: {bug4.CurrentState}");

            Console.WriteLine("\n--- Сценарий 5: Отложить и вернуть ---");
            var bug5 = new Bug();
            bug5.Assign();
            bug5.Defer();
            bug5.Assign();
            bug5.Fix();
            bug5.Verify();
            Console.WriteLine($"Итог: {bug5.CurrentState}");
        }
    }
}
