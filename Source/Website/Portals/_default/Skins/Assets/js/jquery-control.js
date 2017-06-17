$(function()
{
    $.widget("custom.combobox", {
        _create: function()
        {
            this.wrapper = $("<div>")
                .addClass("c-square input-group")
                .insertAfter(this.element);

            this._createAutocomplete();
            this._createShowAllButton();
            this.element.hide();
        },

        _createAutocomplete: function()
        {
            var selected = this.element.children(":selected"),
                value = selected ? selected.text() : "";
            var wrapper = this.wrapper;
            var isDisable = this.element.prop("disabled");

            this.input = $("<input>")
                .appendTo(this.wrapper)
                .val(value)
                .addClass(this.element.attr("class"))
                .attr("placeholder", this.element.attr("placeholder"))
                .prop("disabled", isDisable)
                .on("focus click", function()
                {
                    $(this).select().autocomplete("search", "");
                })
                .autocomplete({
                    disabled: isDisable,
                    delay: 0,
                    minLength: 0,
                    source: $.proxy(this, "_source"),
                    close: function()
                    {
                        $(".ui-helper-hidden-accessible").remove();
                    },
                    open: function()
                    {
                        $(this).autocomplete("widget").css({
                            "width": (wrapper.width() + "px")
                        });
                    }
                });


            this.input.data("ui-autocomplete")._renderItem = function (ul, item)
            {
                var className = item.option.disabled ? "ui-state-disabled" : "";
                    return $("<li>")
                        .addClass(className)
                        .append("<div tabindex='-1' class='ui-menu-item-wrapper'>" + item.label + "</div>")
                        .appendTo(ul);
                };


            this._on(this.input, {
                autocompleteselect: function(event, ui)
                {
                    if (ui.item.option.value === this.element.val())
                    {
                        return;
                    }

                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                    this.element.trigger("change");
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function()
        {
            var input = this.input,
                wasOpen = false;

            var container = $("<span>")
                .addClass("input-group-addon combobox-button")
                .attr("tabIndex", -1)
                .appendTo(this.wrapper)
                .on("mousedown", function()
                {
                    wasOpen = input.autocomplete("widget").is(":visible");
                })
                .on("click", function()
                {
                    input.trigger("focus");

                    // Close if already visible
                    if (wasOpen)
                    {
                        return;
                    }

                    // Pass empty string as value to search for, displaying all results
                    input.autocomplete("search", "");
                });

            // Load icon
            $("<i>")
                .addClass("fa fa-caret-down")
                .appendTo(container);
        },

        _source: function(request, response)
        {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function()
            {
                var text = $(this).text();
                if (typeof this.value !== "undefined"
                    && this.value !== null
                    && (!request.term || matcher.test(text)))
                {
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
                }

                return null;
            }));
        },

        _removeIfInvalid: function(event, ui)
        {
            // Selected an item, nothing to do
            if (ui.item)
            {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function()
            {
                if ($(this).text().toLowerCase() === valueLowerCase)
                {
                    this.selected = valid = true;
                    return false;
                }

                return true;
            });

            // Found a match, nothing to do
            if (valid)
            {
                this.element.trigger("change");
                return;
            }

            // Remove invalid value, reverse back to old value
            this.input.val(this.element.context.selectedOptions[0].text);
        },

        _destroy: function()
        {
            this.wrapper.remove();
            this.element.show();
        }
    });

    $.prototype.calendar = function () {
        $.each($(this), function () {
            var control = $(this);
            var container = $("<div class='calendar-container'></div>");
            var icon = $("<i class='fa fa-calendar calendar-icon'></i>");
            var input = $("<input class='calendar-input form-control c-theme' type='textbox' />");

            // Render
            control.wrap(container);
            input.insertBefore(control);
            icon.insertBefore(control);

            // Attach Events
            var currentDate = moment();
            input.daterangepicker(
                {
                    singleDatePicker: true,
                    showDropdowns: true,
                    startDate: currentDate,
                    locale: {
                        format: "DD/MM/YYYY"
                    }
                }, function (date) {
                    control.val(date.format("YYYYMMDD"));
                });
            icon.click(function () {
                input.click();
            });
            control.val(currentDate.format("YYYYMMDD")).hide();
        });
    };
});