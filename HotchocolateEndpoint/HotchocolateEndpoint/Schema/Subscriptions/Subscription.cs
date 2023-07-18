using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotchocolateEndpoint.Schema.Mutations;
using HotchocolateEndpoint.Schema.Queries;

namespace HotchocolateEndpoint.Schema.Subscriptions;

public class Subscription
{
    [Subscribe]
    public CourseResult CourseCreated([EventMessage] CourseResult course) => course;
    
    public ValueTask<ISourceStream<CourseResult>> SubscribeToCourseUpdate(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
    {
        string topicName = $"{courseId}_CourseUpdated";
        return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
    }
    
    [Subscribe(With = nameof(SubscribeToCourseUpdate))]
    public CourseResult CourseUpdated([EventMessage] CourseResult course)
    {
        return course;
    }
}