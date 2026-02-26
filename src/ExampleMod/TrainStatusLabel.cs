using Mafi.Core;
using Mafi.Core.Trains;
using Mafi.Localization;

namespace ExampleMod;

public class TrainStatusLabel
{
    public static LocStrFormatted GetLabelForStatus(TrainStateForUi state)
    {
        switch (state)
        {
          case TrainStateForUi.Paused:
            return Tr.Paused.AsFormatted;
          case TrainStateForUi.Driving:
            return Tr.TrainStatus_Driving.AsFormatted;
          case TrainStateForUi.NoLineSet:
            return Tr.TrainStatus_NoLine.AsFormatted;
          case TrainStateForUi.LineHasNoStations:
            return Tr.TrainStatus_EmptyLine.AsFormatted;
          case TrainStateForUi.NoValidGoals:
            return Tr.TrainStatus_NoValidGoal.AsFormatted;
          case TrainStateForUi.WaitingForFreeTrack:
            return Tr.TrainStatus_WaitingForFreeTrack.AsFormatted;
          case TrainStateForUi.LoadingOrUnloading:
            return Tr.TrainStatus_AtStation.AsFormatted;
          case TrainStateForUi.Arriving:
            return Tr.TrainStatus_Arriving.AsFormatted;
          case TrainStateForUi.Departing:
            return Tr.TrainStatus_Departing.AsFormatted;
          case TrainStateForUi.NoPower:
            return Tr.TrainStatus_NoPower.AsFormatted;
          case TrainStateForUi.WaitingForDepotDoors:
            return Tr.TrainStatus_WaitingForDepotDoors.AsFormatted;
          case TrainStateForUi.CannotFindPath:
            return Tr.TrainStatus_CannotFindPath.AsFormatted;
          case TrainStateForUi.WaitingForSuperBlock:
            return Tr.TrainStatus_WaitingForSuperBlock.AsFormatted;
          case TrainStateForUi.WaitingForBidirectionalSuperBlock:
            goto case TrainStateForUi.WaitingForSuperBlock;
          case TrainStateForUi.ArrivalConditionsNotMet:
            return Tr.TrainStatus_ArrivalConditionsNotMet.AsFormatted;
          case TrainStateForUi.SelfIntersect:
            return Tr.TrainStatus_SelfIntersect.AsFormatted;
          case TrainStateForUi.AtOnlyStationOnLine:
            return Tr.TrainStatus_NoOtherStation.AsFormatted;
          case TrainStateForUi.Unknown:
          default:
            return LocStrFormatted.Empty;
        }
    }
}