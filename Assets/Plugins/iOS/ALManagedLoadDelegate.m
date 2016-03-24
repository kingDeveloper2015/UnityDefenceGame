//
//  ALTypeRememberingLoadDelegate.m
//  AppLovin Unity Extension
//
//  Created by Matt Szaro on 5/20/14.
//  Copyright (c) 2014 AppLovin. All rights reserved.
//

#import "ALManagedLoadDelegate.h"

@interface ALManagedLoadDelegate ()

@property (strong, nonatomic) ALAdSize* size;
@property (strong, nonatomic) ALAdType* type;
@property (strong, nonatomic) id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate> wrapper;

-(instancetype) initWithSize: (ALAdSize*) size type: (ALAdType*) type wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>) wrapper;

@end

static NSMutableSet* managedLoadDelegates;

@implementation ALManagedLoadDelegate

+(ALManagedLoadDelegate*) sharedDelegateForSize:(ALAdSize *)size type:(ALAdType *)type wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>) wrapper
{
    if(!managedLoadDelegates)
    {
        managedLoadDelegates = [NSMutableSet set];
        [managedLoadDelegates retain];
    }
    
    ALManagedLoadDelegate* delegate = [[self class] retrieveDelegateForSize: size type: type wrapper: wrapper];
    
    if(!delegate)
    {
        delegate = [[self class] createDelegateForSize: size type: type wrapper: wrapper];
    }
    
    return delegate;
}

+(ALManagedLoadDelegate*) retrieveDelegateForSize: (ALAdSize*) size type: (ALAdType*) type wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>) wrapper
{
    for(ALManagedLoadDelegate* delegate in managedLoadDelegates)
    {
        if([delegate.size isEqual: size] && [delegate.type isEqual: type] && [delegate.wrapper isEqual: wrapper])
        {
            return delegate;
        }
    }
    
    return nil;
}

+(ALManagedLoadDelegate*) createDelegateForSize: (ALAdSize*) size type: (ALAdType*) type wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>) wrapper
{
    ALManagedLoadDelegate* delegate = [[ALManagedLoadDelegate alloc] initWithSize: size type: type wrapper: wrapper];
    [managedLoadDelegates addObject: delegate];
    
    return delegate;
}

-(instancetype) initWithSize: (ALAdSize*) aSize type: (ALAdType*) aType wrapper: (id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>) aWrapper
{
    self = [super init];
    if(self)
    {
        self.size = aSize;
        self.type = aType;
        self.wrapper = aWrapper;
    }
    return self;
}

-(void) adService:(ALAdService *)adService didLoadAd:(ALAd *)ad
{
    // Forward this through to the wrapper.
    [self.wrapper adService: adService didLoadAd: ad];
}

-(void) adService:(ALAdService *)adService didFailToLoadAdWithError:(int)code
{
    // Pass this typed load fail info to the wrapper.
    [self.wrapper adService: adService didFailToLoadAdOfSize: self.size type: self.type withError: (NSInteger) code];
}

-(NSString*) description
{
    return [NSString stringWithFormat: @"%@ - %@/%@/%@", @"ALManagedLoadDelegate", self.size, self.type, self.wrapper];
}

@end
