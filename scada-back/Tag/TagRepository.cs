using MongoDB.Bson;
using MongoDB.Driver;
using scada_back.Database;
using scada_back.Exception;

namespace scada_back.Tag;

public class TagRepository : ITagRepository
{
    private readonly IMongoCollection<Model.Abstraction.Tag> _tags;

    public TagRepository(IScadaDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _tags = database.GetCollection<Model.Abstraction.Tag>(settings.TagsCollectionName);

    }
    
    public async Task<IEnumerable<Model.Abstraction.Tag>> GetAll()
    {
        return (await _tags.FindAsync(tag => true)).ToList();
    }

    public async Task<IEnumerable<Model.Abstraction.Tag>> GetAll(string discriminator)
    {
        var filter = Builders<Model.Abstraction.Tag>.Filter.Eq("_t", discriminator);
        return (await _tags.FindAsync<Model.Abstraction.Tag>(filter)).ToList();
    }

    public async Task<IEnumerable<string>> GetAllNames(string signalType)
    {
        var filter = Builders<Model.Abstraction.Tag>.Filter.Regex("_t", new BsonRegularExpression($"^{signalType}"));
        IEnumerable<Model.Abstraction.Tag> tags = (await _tags.FindAsync(filter)).ToList();
        return tags.Select(tag => tag.TagName).ToList();
    }

    public async Task<Model.Abstraction.Tag> Get(string tagName)
    {
        return (await _tags.FindAsync(tag => tag.TagName == tagName)).FirstOrDefault();
    }

    public async Task<Model.Abstraction.Tag> Create(Model.Abstraction.Tag newTag)
    {
        await _tags.InsertOneAsync(newTag);
        Model.Abstraction.Tag tag = await Get(newTag.TagName);
        if (tag == null)
        {
            throw new ActionNotExecutedException("Create failed.");
        }
        return tag;
    }

    public async Task<Model.Abstraction.Tag> Delete(string tagName)
    {
        Model.Abstraction.Tag toBeDeleted = await Get(tagName);
        if (toBeDeleted == null) {
            throw new ObjectNotFoundException($"Tag with {tagName} doesn't exist");
        }
        DeleteResult result = await _tags.DeleteOneAsync(tag => tag.TagName == tagName);
        if (result.DeletedCount == 0)
        {
            throw new ActionNotExecutedException("Delete failed.");
        }
        return toBeDeleted;
    }

    public async Task<Model.Abstraction.Tag> Update(Model.Abstraction.Tag updatedTag)
    {
        Model.Abstraction.Tag oldTag = Get(updatedTag.TagName).Result;
        if (oldTag == null) {
            throw new ObjectNotFoundException($"Tag with {updatedTag.TagName} doesn't exist");
        }
        updatedTag.Id = oldTag.Id;
        ReplaceOneResult result = await _tags.ReplaceOneAsync(tag => tag.Id == updatedTag.Id, updatedTag);
        if (result.ModifiedCount == 0)
        {
            throw new ActionNotExecutedException("Update failed.");
        }
        return await Get(updatedTag.TagName);
    }
}