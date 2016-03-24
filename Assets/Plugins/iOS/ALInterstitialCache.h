//
//  ALInterstitialCache.h
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/17/14.
//
//

#import <Foundation/Foundation.h>
#import "ALAdLoadDelegate.h"
#import "ALInterstitialCache.h"
#import "ALAdDelegateWrapper.h"
#import "ALManagedLoadDelegate.h"

@interface ALInterstitialCache : NSObject  <ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>

+(instancetype) shared;

@property (strong, nonatomic) ALAd* lastAd;
@property (strong, nonatomic) ALAdDelegateWrapper* wrapperToNotify;

@end
