//
//  ALInterstitialCache.m
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/17/14.
//
//

#import "ALInterstitialCache.h"
#import "ALAd.h"
#import "ALAdLoadDelegate.h"

static ALInterstitialCache* instance;

@implementation ALInterstitialCache
@synthesize lastAd, wrapperToNotify;

+(instancetype) shared
{
    if(!instance)
    {
        instance = [[ALInterstitialCache alloc] init];
    }
    return instance;
}

-(void) adService:(ALAdService *)adService didLoadAd:(ALAd *)ad
{
    lastAd = ad;
    [lastAd retain];
    [wrapperToNotify adService: adService didLoadAd: ad];
}

-(void) adService:(ALAdService *)adService didFailToLoadAdWithError:(int)code
{
    [wrapperToNotify adService: adService didFailToLoadAdWithError: code];
}

-(void) adService:(ALAdService *)adService didFailToLoadAdOfSize:(ALAdSize *)size type:(ALAdType *)type withError:(NSInteger)error
{
    [wrapperToNotify adService: adService didFailToLoadAdOfSize: size type: type withError: error];
}

@end
