using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tilegram.Feature.Feed
{
	public interface IFeedService
	{
		Task<Either<Exception, List<FeedItem>>> UserFeed();
	}
}