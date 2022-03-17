﻿using HttpApi.Host.Services.NotificationInterfaces;
using MediatR;

namespace HttpApi.Host.Services.GameServices;

public record GameFinishedNotificationRequest : INotification;

internal class GameFinishedNotificationHandler : INotificationHandler<GameFinishedNotificationRequest>
{
  private readonly IGameIsFinished _gameIsFinished;

  public GameFinishedNotificationHandler ( IGameIsFinished gameIsFinished )
  {
    _gameIsFinished = gameIsFinished;
  }
  public async Task Handle ( GameFinishedNotificationRequest finishedNotificationRequest, CancellationToken cancellationToken )
  {
    await _gameIsFinished.NotifyGameIsFinished ();
  }
}


public record PlaceOpenedNotificationRequest () : INotification;

internal class PlaceOpenedNotificationHandler : INotificationHandler<PlaceOpenedNotificationRequest>
{
  private readonly IPlaceOpenedNotification _placeOpenedNotification;

  public PlaceOpenedNotificationHandler ( IPlaceOpenedNotification placeOpenedNotification )
  {
    _placeOpenedNotification = placeOpenedNotification;
  }
  public async Task Handle ( PlaceOpenedNotificationRequest notification, CancellationToken cancellationToken )
  {
    await _placeOpenedNotification.NotifyPlaceOpened ();
  }
}

