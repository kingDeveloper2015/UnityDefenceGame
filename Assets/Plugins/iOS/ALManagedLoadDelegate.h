//
//  ALTypeRememberingLoadDelegate.h
//  AppLovin Air Extension
//
//  Created by Matt Szaro on 5/20/14.
//  Copyright (c) 2014 AppLovin. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ALAdLoadDelegate.h"

@protocol ALUnityTypedLoadFailureDelegate <NSObject>

-(void) adService:(ALAdService *)adService didFailToLoadAdOfSize: (ALAdSize*) size type: (ALAdType*) type withError: (NSInteger) error;

@end

@interface ALManagedLoadDelegate : NSObject <ALAdLoadDelegate>

+(ALManagedLoadDelegate*) sharedDelegateForSize: (ALAdSize*) size type: (ALAdType*) type wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>) wrapper;
- (id)init __attribute__((unavailable("Use +sharedDelegate.. instead of alloc-init pattern.")));

@end
