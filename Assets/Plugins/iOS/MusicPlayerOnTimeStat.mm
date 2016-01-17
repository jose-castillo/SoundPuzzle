//#import <math.h>
#import <MediaPlayer/MediaPlayer.h>

extern "C" void UnitySendMessage(const char *, const char *, const char *);

@interface MusicPlayerOnTimeStat : NSObject
{
	MPMusicPlayerController		*musicPlayer;
}
@end

@implementation MusicPlayerOnTimeStat

- (void) handle_PlaybackStateChanged: (id) notification
{

	NSLog(@"MusicPlayerOnTimeStat:handle_PlaybackStateChanged: id=%@", notification);

	if (musicPlayer) {
		NSString* sendGameObjectName = @"SoundManager";
		NSString* sendFunctionName = @"CallbackMusicPlayerOnTimeStat";
		NSString* sendStatPaused = @"Paused";
		NSString* sendStatPlaying = @"Playing";
		NSString* sendStatStopped = @"Stopped";
		MPMusicPlaybackState playbackState = [musicPlayer playbackState];
	
		if (playbackState == MPMusicPlaybackStatePaused) {

			//NSLog(@"MusicPlayerOnTimeStat:handle_PlaybackStateChanged: Paused");
			UnitySendMessage((char *)[sendGameObjectName UTF8String], (char *)[sendFunctionName UTF8String], (char *)[sendStatPaused UTF8String]);

		} else if (playbackState == MPMusicPlaybackStatePlaying) {

			//NSLog(@"MusicPlayerOnTimeStat:handle_PlaybackStateChanged: Playing");
			UnitySendMessage((char *)[sendGameObjectName UTF8String], (char *)[sendFunctionName UTF8String], (char *)[sendStatPlaying UTF8String]);

		} else if (playbackState == MPMusicPlaybackStateStopped) {

			//NSLog(@"MusicPlayerOnTimeStat:handle_PlaybackStateChanged: Stopped");
			UnitySendMessage((char *)[sendGameObjectName UTF8String], (char *)[sendFunctionName UTF8String], (char *)[sendStatStopped UTF8String]);

		}
		//NSLog(@"MusicPlayerOnTimeStat:handle_PlaybackStateChanged: state=%d", playbackState);

	} else {

		//NSLog(@"MusicPlayerOnTimeStat:handle_PlaybackStateChanged: musicPlayer is null");

	}
}

- (id) init
{
	self = [super init];

	musicPlayer = [MPMusicPlayerController iPodMusicPlayer];

	if (musicPlayer) {

		NSNotificationCenter *notificationCenter = [NSNotificationCenter defaultCenter];

		[notificationCenter addObserver: self
							   selector: @selector (handle_PlaybackStateChanged:)
								   name: MPMusicPlayerControllerPlaybackStateDidChangeNotification
								 object: musicPlayer];

		[musicPlayer beginGeneratingPlaybackNotifications];
	} else {
		//NSLog(@"MusicPlayerOnTimeStat:registNotification: musicPlayer is null");
	}

	return self;
}

- (void) dealloc
{

	//NSLog(@"MusicPlayerOnTimeStat:unregistNotification");

	if (musicPlayer) {
		[[NSNotificationCenter defaultCenter] removeObserver: self
														name: MPMusicPlayerControllerPlaybackStateDidChangeNotification
														object: musicPlayer];
	}
	[super dealloc];

}
@end

extern "C" {
	void *_MusicPlayerOnTimeStat_Init();
	void _MusicPlayerOnTimeStat_Destroy(void *instance);
}

void *_MusicPlayerOnTimeStat_Init()
{
	id instance = [[MusicPlayerOnTimeStat alloc] init];
	return (void *)instance;
}

void _MusicPlayerOnTimeStat_Destroy(void *instance)
{
	MusicPlayerOnTimeStat *pluginClass = (MusicPlayerOnTimeStat *)instance;
	[pluginClass release];
}

