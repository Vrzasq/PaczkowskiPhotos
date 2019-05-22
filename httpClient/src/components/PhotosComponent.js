import Services from '../services.js';
import Commons from '../commons.js';
import PhotoViewComponent from './PhotoViewComponent.js';

export default {
    components: {
        PhotoViewComponent
    },
    template: `
    <div class="photo-display">
        <h2 class="w3-blue-grey" v-once>{{ title }}</h2>
        <div class="categories-container">
            <h3 class="w3-blue-grey">{{ categoriesTitle }}</h3>
            <div class="category" v-for="category in categories">
                <span>{{ category.name }}</span>
                <img @click="showCategory(category.name)" src="images/folder.png" />
            </div>
        </div>
        <div class="photos-container">
            <h3 class="w3-blue-grey">{{ unCategoriesTitle }}</h3>
            <photo-view-component v-for="image, index in images"
                :image="image.image"
                :imageName="image.displayName"
                :imageId="image.photoNum"
                :key="image.photoNum"
                @delete="deletePhoto($event, index)"
            ></photo-view-component>
        </div>        
    </div>
    `,

    data () {
        return {
            title: 'MY PHOTOS',
            categoriesTitle: 'CATEGORIES',
            unCategoriesTitle: 'NOT CATEGORIZED',
            images: [],
            categories: []
        };
    },

    methods: {
        deletePhoto(imageData, index) {
            let vm = this;
            $.ajax({
                method: 'DELETE',
                contentType: 'application/json',
                xhrFields: { withCredentials: true },
                url: Services.photo.deletePhoto,
                data: JSON.stringify({ photoNum: imageData.imageId }),
                success: function (data) {
                    if (data) {
                        alert('SUCCESS');
                        vm.$delete(vm.images, index);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) { console.log(`${textStatus} ${errorThrown}`); }
            });
        },

        showCategory(category) {
            //TODO add logic for switchich to category view
            alert(category);
        },

        getCategories() {
            let vm = this;
            $.ajax({
                method: 'GET',
                xhrFields: { withCredentials: true },
                url: Services.photo.getCategories,
                success: function (data) {
                    console.log(data);
                    vm.categories = data;
                },
                error: function (jqXHR, textStatus, errorThrown) { console.log(`${textStatus} ${errorThrown}`); }
            });
        },

        getPhotos() {
            let vm = this;
            $.ajax({
                method: 'GET',
                xhrFields: { withCredentials: true },
                url: Services.photo.getUncategorizedPhotos,
                success: function (data) {
                    console.log(data);
                    vm.images = data;
                },
                error: function (jqXHR, textStatus, errorThrown) { console.log(`${textStatus} ${errorThrown}`); }
            });
        }
    },

    mounted: function () {
        this.getCategories();
        this.getPhotos();
    },
}